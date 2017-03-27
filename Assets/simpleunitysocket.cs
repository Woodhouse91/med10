using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using UnityEngine;
using System.Threading;

public class simpleunitysocket : MonoBehaviour
{
    Thread CLIENT;
    static int port = 9002;
    static TcpClient client;
    static IPHostEntry ipHostInfo = Dns.GetHostEntry(IPAddress.Loopback);
    static IPAddress ipAddress = ipHostInfo.AddressList[0];
    static IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

    private void Start()
    {
        client = new TcpClient();
        CLIENT = new Thread(run);
    }
    void run()
    {
        
    }
}