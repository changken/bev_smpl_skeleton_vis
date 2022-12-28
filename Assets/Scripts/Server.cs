using UnityEngine;
using System.Net.Sockets;
using System.Collections;

public class Server : MonoBehaviour
{
    private ServerThread st;
    private bool isSend;//儲存是否發送訊息完畢

    private void Start()
    {
        //開始連線，設定使用網路、串流、TCP
        st = new ServerThread(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, "127.0.0.1", 8000);
        st.Listen();//讓Server socket開始監聽連線
        st.StartConnect();//開啟Server socket
        isSend = true;
    }

    private void Update()
    {
        if (st.receiveMessage != null)
        {
            Debug.Log("Client:" + st.receiveMessage);
            st.receiveMessage = null;
        }
        if (isSend == true)
            StartCoroutine(delaySend());//延遲發送訊息

        st.Receive();
    }

    private IEnumerator delaySend()
    {
        isSend = false;
        yield return new WaitForSeconds(1);//延遲1秒後才發送
        st.Send("Hello~ My name is Server");
        isSend = true;
    }


    private void OnApplicationQuit()//應用程式結束時自動關閉連線
    {
        st.StopConnect();
    }
}