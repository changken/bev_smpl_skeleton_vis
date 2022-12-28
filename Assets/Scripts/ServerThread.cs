using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


class ServerThread
{
    //���c�A�x�sIP�MPort
    private struct Struct_Internet
    {
        public string ip;
        public int port;
    }

    private Socket serverSocket;//���A��������Socket
    private Socket clientSocket;//�s�u�ϥΪ�Socket
    private Struct_Internet internet;//�ŧi���c����
    public string receiveMessage;
    private string sendMessage;

    private Thread threadConnect;//�s�u��Thread
    private Thread threadReceive;//������ƪ�Thread

    public ServerThread(AddressFamily family, SocketType socketType, ProtocolType protocolType, string ip, int port)
    {
        serverSocket = new Socket(family, socketType, protocolType);//new server socket object
        internet.ip = ip;//�x�sIP
        internet.port = port;//�x�sPort
        receiveMessage = null;//��l�Ʊ��������
    }

    //�}�l��ť�s�u�ݨD
    public void Listen()
    {
        //���A��������IP�MPort
        serverSocket.Bind(new IPEndPoint(IPAddress.Parse(internet.ip), internet.port));
        serverSocket.Listen(1);//�̦h�@�������h�֤H�s�u
    }

    //�}�l�s�u
    public void StartConnect()
    {
        //�ѩ�s�u���\���e�{�����|���U�A�ҥH�����ϥ�Thread
        threadConnect = new Thread(Accept);
        threadConnect.IsBackground = true;//�]�w���I��������A��{�������ɷ|�۰ʵ���
        threadConnect.Start();
    }

    //����s�u
    public void StopConnect()
    {
        try
        {
            clientSocket.Close();
        }
        catch (Exception)
        {

        }
    }

    //�H�e�T��
    public void Send(string message)
    {
        if (message == null)
            throw new NullReferenceException("message���i��Null");
        else
            sendMessage = message;
        SendMessage();//�ѩ��ƶǻ��t�׫ܧ֡A�S���n�ϥ�Thread
    }

    public void Receive()
    {
        //���P�_�����threadReceive�Y�٦b���汵���ɮת��u�@�A�h��������
        if (threadReceive != null && threadReceive.IsAlive == true)
            return;
        //�ѩ�b������Ҧ���ƫe���|���U�A�ҥH�����ϥ�Thread
        threadReceive = new Thread(ReceiveMessage);
        threadReceive.IsBackground = true;//�]�w���I��������A��{�������ɷ|�۰ʵ���
        threadReceive.Start();
    }

    private void Accept()
    {
        try
        {
            clientSocket = serverSocket.Accept();//����s�u���\��~�|���U����
            //�s�u���\��A�Y�O���Q�A������L�s�u�A�i�H����serverSocket
            //serverSocket.Close();
        }
        catch (Exception)
        {

        }
    }

    private void SendMessage()
    {
        try
        {
            if (clientSocket != null && clientSocket.Connected == true)//�Y���\�s�u�~�ǻ����
            {
                //�N��ƶi��s�X���ରByte��ǻ�
                clientSocket.Send(Encoding.ASCII.GetBytes(sendMessage));
            }
        }
        catch (Exception)
        {

        }
    }

    private void ReceiveMessage()
    {
        if (clientSocket != null && clientSocket.Connected == true)
        {
            // 54 * 3 * 11 = 1944
            byte[] bytes = new byte[4096];//�Ψ��x�s�ǻ��L�Ӫ����
            long dataLength = clientSocket.Receive(bytes);//��Ʊ����������e���|���b�o��
            //dataLength���ǻ��L�Ӫ�"��ƪ���"

            receiveMessage = Encoding.ASCII.GetString(bytes);//�N�ǹL�Ӫ���ƸѽX���x�s
        }
    }
}

