using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;

public class AnimationCode : MonoBehaviour
{
    public GameObject[] Body;
    List<string> lines;
    int counter = 0;
    
    void Start()
    {
    	// 读取MotionFile_Pose.txt的动作数据文件
        lines = System.IO.File.ReadLines("Assets/MotionFiles/MotionFile2.txt").ToList();
        
    }

    void Update()
    {
        string[] points = lines[counter].Split(',');
	
	// 循环遍历到每一个Sphere点
        for (int i =0; i<24;i++)
        {
            float x = float.Parse(points[0 + (i * 3)]) * 10;
            float y = -1 * float.Parse(points[1 + (i * 3)]) * 10; //乘-1
            float z = float.Parse(points[2 + (i * 3)]) * 10;
            Body[i].transform.localPosition = new Vector3(x, y, z);
        }

        counter += 1;
        if (counter == lines.Count) { counter = 0; }
        Thread.Sleep(30);
    }
}
