using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Threading;
using System.Net.Sockets;
using System;


public class AnimationCode : MonoBehaviour
{
    public GameObject[] Body;
    List<string> lines;
    int counter = 0;

    public string JointPointFile;

    //socket server
    private ServerThread st;
    public string Socket_Host = "127.0.0.1";
    public int Socket_Port = 8000;

    void Start()
    {
        //開始連線，設定使用網路、串流、TCP
        st = new ServerThread(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, Socket_Host, Socket_Port);
        st.Listen();//讓Server socket開始監聽連線
        st.StartConnect();//開啟Server socket
    }

    void Update()
    {
        if (st.receiveMessage != null)
        {
            Debug.Log("Client:" + st.receiveMessage);

            string[] points = st.receiveMessage.Split(',');

            try { 
                // 循环遍历到每一个Sphere点
                for (int i = 0; i < this.Body.Length; i++)
                {
                    float x = float.Parse(points[0 + (i * 3)]) * 10;
                    float y = -1 * float.Parse(points[1 + (i * 3)]) * 10; //乘-1
                    float z = float.Parse(points[2 + (i * 3)]) * 10;
                    Body[i].transform.localPosition = new Vector3(x, y, z);
                }
            }catch(Exception e)
            {
                Debug.Log("Error");
            }

            st.receiveMessage = null;
        }

        st.Receive();
    }

    private void OnApplicationQuit()//應用程式結束時自動關閉連線
    {
        st.StopConnect();
    }
}
