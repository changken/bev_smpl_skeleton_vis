using UnityEngine;
using System.Net.Sockets;
using System.Collections;

public class Server : MonoBehaviour
{
    private ServerThread st;
    private bool isSend;//�x�s�O�_�o�e�T������

    private void Start()
    {
        //�}�l�s�u�A�]�w�ϥκ����B��y�BTCP
        st = new ServerThread(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp, "127.0.0.1", 8000);
        st.Listen();//��Server socket�}�l��ť�s�u
        st.StartConnect();//�}��Server socket
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
            StartCoroutine(delaySend());//����o�e�T��

        st.Receive();
    }

    private IEnumerator delaySend()
    {
        isSend = false;
        yield return new WaitForSeconds(1);//����1���~�o�e
        st.Send("Hello~ My name is Server");
        isSend = true;
    }


    private void OnApplicationQuit()//���ε{�������ɦ۰������s�u
    {
        st.StopConnect();
    }
}