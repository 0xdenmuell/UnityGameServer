using System;
using System.Net.Sockets;

/*
 * This class starts a TCP listener for clients to connect
*/


namespace UnityGameServer
{
    public class Client
    {
        public int connectionID;
        public int ipAddress;
        public TcpClient socket;
        public NetworkStream stream;
        private byte[] recBuffer;
        public ByteBuffer buffer;
        public string username;
        public bool OnServerAlready;
        public int packetID;

        
        //The Server starts listening for the first Message of a Client
        public void Start()
        {
            socket.SendBufferSize = 4096;
            socket.ReceiveBufferSize = 4096;
            stream = socket.GetStream();
            recBuffer = new byte[4096];

            stream.BeginRead(recBuffer, 0, socket.ReceiveBufferSize, OnReceiveData, null);
            Console.WriteLine("Incoming Connection form '{0}'.", socket.Client.RemoteEndPoint.ToString());
        }

        //If a Client is connected to the Server, it gets a callback
        //This is an endless method until the client disconnects
        private void OnReceiveData(IAsyncResult result)
        {
            try
            {
                int length = stream.EndRead(result);
              
                
                if (length <= 0)
                {
                    CloseConnection(socket);
                    return;
                }

                byte[] newBytes = new byte[length];
                Array.Copy(recBuffer, newBytes, length);

                ServerHandleData.HandleData(connectionID, newBytes);

                stream.BeginRead(recBuffer, 0, socket.ReceiveBufferSize, OnReceiveData, null);
            }
            catch (ObjectDisposedException clientHasDisconnected)
            {
                stream.Close();
                socket.Close();
                ClientManager.client.Remove(connectionID);
                return;
            }
            catch (Exception unknownDisconnect)
            {
                Console.WriteLine(unknownDisconnect);
                ClientManager.client.Remove(connectionID);
                CloseConnection(socket);
                return;
            }
        }

        //Function which Closes the Connection with 
        //Client so the Server doesnt shutdown
        public static void CloseConnection(TcpClient socket)
        {
            Console.WriteLine("Connection from '{0}' has been terminated.", socket.Client.RemoteEndPoint.ToString());
            socket.Close();
        }
    }
}
