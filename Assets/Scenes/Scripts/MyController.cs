using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;

public class MyController : MonoBehaviour
{
    public GameObject obj;
    private bool flag = false;
    private MMD4MecanimBoneImpl[] boneList;
    private int[] mappingTable = kizunaaiMappingTable();

    public string JointPointFile;
    List<string> lines;
    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 读取MotionFile_Pose.txt的动作数据文件
        lines = System.IO.File.ReadLines("Assets/MotionFiles/bev/" + JointPointFile + ".txt").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(o.GetComponent<MMD4MecanimModel>().boneList.Length);
        if(obj.GetComponent<MMD4MecanimModel>().boneList.Length > 0 && !flag)
        {
            boneList = obj.GetComponent<MMD4MecanimModel>().boneList;
            flag = true;
            foreach(MMD4MecanimBoneImpl bone in boneList)
            {
                Debug.Log("=======================");
                Debug.Log(bone.name);
                Debug.Log(bone.transform.position);
            }
        }

        if (flag)
        {
            string[] points = lines[counter].Split(',');
            int offset = 3;

            boneList[mappingTable[0]].transform.position =
                new Vector3(float.Parse(points[0]), float.Parse(points[1]), float.Parse(points[2]));

            // 循环遍历到每一个Sphere点
            for (int i = 0; i < mappingTable.Length; i++)
            {
                //Debug.Log(offset + 0 + (i * 4));
                float x = float.Parse(points[offset + 0 + (i * 4)]);
                float y = float.Parse(points[offset + 1 + (i * 4)]);
                float z = float.Parse(points[offset + 2 + (i * 4)]);
                float w = float.Parse(points[offset + 3 + (i * 4)]);

                Quaternion quaternion = new Quaternion(x, y, z, w);
                Quaternion quat_x_180_cw = Quaternion.AngleAxis(-180, new Vector3(1.0f, 0.0f, 0.0f));

                /*
                 float x = float.Parse(points[0 + (i * 3)]) * 50;
                 float y = float.Parse(points[1 + (i * 3)]) * 50; //乘-1
                 float z = float.Parse(points[2 + (i * 3)]) * 5;
                 */
                if (mappingTable[i] != -1)
                {
                    if(i == 0)
                    {
                        //quaternion.SetAxisAngle(new Vector3(1.0f, 0.0f, 0.0f), -180);
                        quaternion = quaternion * quat_x_180_cw;
                    }
                        //boneList[mappingTable[i]].GetComponent<MMD4MecanimBoneImpl>().userRotation = quaternion;
                        boneList[mappingTable[i]].GetComponent<MMD4MecanimBoneImpl>().userEulerAngles = quaternion.eulerAngles;
                }
            }

            counter += 1;
            if (counter == lines.Count) { counter = 0; }
            Thread.Sleep(30);
        }

        
    }

    private static int[] kizunaaiMappingTable()
    {
        /*
         smpl index => kizuna ai index
         */
        int[] mappingTable = new int[24]
        {
            74, 82, 77, 4, 83, 78, 5, 84, 79, -1, -1, -1, 7, 42, 10, 8, 43, 11, 48, 16, 53, 21, -1, -1
        };
        return mappingTable;
    }
}
