using System;
using System.Net;
using System.Net.Sockets;

namespace UnityGameServer
{

    static class ServerTCP
    {
        static TcpListener serverSocket = new TcpListener(IPAddress.Any, 5557);

        //Adds all available Packets of the Client and starts accepting Clients
        public static void InititalizeNetwork()
        { 
            Console.WriteLine("Initializing Packets ...");
            ServerHandleData.InitializePackets();
            serverSocket.Start();
            serverSocket.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
        }

        //Callback if an Client connects
        private static void OnClientConnect(IAsyncResult result)
        {
            TcpClient client = serverSocket.EndAcceptTcpClient(result);
            serverSocket.BeginAcceptTcpClient(new AsyncCallback(OnClientConnect), null);
            ClientManager.CreateNewConnetion(client);
        }
    }
}
