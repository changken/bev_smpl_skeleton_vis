using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;

public class MyController2 : MonoBehaviour
{
    private Animator anim;

    private bool flag = false;
    public GameObject[] boneList;
    private HumanBodyBones[] bones = new HumanBodyBones[]
            {
                HumanBodyBones.Hips, HumanBodyBones.LeftUpperLeg, HumanBodyBones.RightUpperLeg,
                HumanBodyBones.Spine, HumanBodyBones.LeftLowerLeg, HumanBodyBones.RightLowerLeg,
                HumanBodyBones.Chest, HumanBodyBones.LeftFoot, HumanBodyBones.RightFoot, HumanBodyBones.UpperChest,
                HumanBodyBones.LeftToes, HumanBodyBones.RightToes, HumanBodyBones.Neck, HumanBodyBones.LeftShoulder, HumanBodyBones.RightShoulder,
                HumanBodyBones.Head, HumanBodyBones.LeftUpperArm, HumanBodyBones.RightUpperArm, HumanBodyBones.LeftLowerArm, HumanBodyBones.RightLowerArm,
                HumanBodyBones.LeftHand, HumanBodyBones.RightHand
            };

    public string JointPointFile;
    List<string> lines;
    int counter = 0;

    // Start is called before the first frame update
    void Start()
    {
        // 读取MotionFile_Pose.txt的动作数据文件
        lines = System.IO.File.ReadLines("Assets/MotionFiles/bev/" + JointPointFile + ".txt").ToList();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //Debug.Log(o.GetComponent<MMD4MecanimModel>().boneList.Length);
        /*
        if(this.boneList.Length > 0 && !flag)
        {
            flag = true;
            foreach(GameObject bone in boneList)
            {
                if (bone != null)
                {

                    Debug.Log("=======================");
                    Debug.Log(bone.name);
                    Debug.Log(bone.transform.position);
                }
            }
        }
        */

        //if (flag)
    }

    private void OnAnimatorIK(int layerIndex)
    {
        
        string[] line_split = lines[counter].Split(';');
        string[] trans = line_split[0].Split(',');
        string[] points = line_split[1].Split(',');

        Transform hipBone = anim.GetBoneTransform(bones[0]);

        hipBone.position =
            new Vector3(float.Parse(trans[0]) * 1, float.Parse(trans[1]) * -1, float.Parse(trans[2]) * -1);

        // 循环遍历到每一个Sphere点
        //boneList.Length
        for (int i = 0; i < bones.Length; i++)
        {
            //Debug.Log(offset + 0 + (i * 4));
            float x = float.Parse(points[0 + (i * 4)]);
            float y = float.Parse(points[1 + (i * 4)]);
            float z = float.Parse(points[2 + (i * 4)]);
            float w = float.Parse(points[3 + (i * 4)]);

            //Quaternion baseRot = Quaternion.Euler(new Vector3(-90f, 0, 0));
            Quaternion quaternion = new Quaternion(x, y, z, w);
            //Quaternion quat_x_180_cw = Quaternion.AngleAxis(-180, new Vector3(1.0f, 0.0f, 0.0f));
            Quaternion quat_x_90_cw = Quaternion.AngleAxis(90, new Vector3(1.0f, 0.0f, 0.0f));
            Quaternion quat_z_90_cw = Quaternion.AngleAxis(90, new Vector3(0.0f, 0.0f, 1.0f));
            
            /*
                float x = float.Parse(points[0 + (i * 3)]) * 50;
                float y = float.Parse(points[1 + (i * 3)]) * 50; //乘-1
                float z = float.Parse(points[2 + (i * 3)]) * 5;
                */

            Transform currentBone = anim.GetBoneTransform(bones[i]);

            if (i == 0)
            {
                //quaternion.SetAxisAngle(new Vector3(1.0f, 0.0f, 0.0f), -180);
                //anim.bodyRotation = (quat_x_90_cw * quat_z_90_cw) * quaternion;
                anim.bodyRotation =  quaternion;
                //boneList[i].transform.rotation = quaternion;
                //boneList[i].transform.rotation = (quat_x_180_cw * quat_z_90_cw) * quaternion;
            }
            else
            {
                anim.SetBoneLocalRotation(bones[i], quaternion);
                //currentBone.localRotation = quaternion;
                //currentBone.rotation = quaternion;
            }

        }

        counter += 1;
        if (counter == lines.Count) { counter = 0; }
        Thread.Sleep(30);
        
        }
}
