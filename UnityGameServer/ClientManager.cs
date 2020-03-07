using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace UnityGameServer
{
    class ClientManager
    {
        public static Dictionary<int, Client> client = new Dictionary<int, Client>();

        static string serverMessage = "";

        //If a Client wants to connect, it gets his own socket
        //The Client gets a first Welcome Message to check on both sides if the connecting is working
        public static void CreateNewConnetion(TcpClient tempClient)
        {
            Client newClient = new Client();
            newClient.socket = tempClient;
            newClient.connectionID = ((IPEndPoint)tempClient.Client.RemoteEndPoint).Port;
            newClient.Start();
            client.Add(newClient.connectionID, newClient);

            DataSender.SendWelcomeMessage(newClient.connectionID);

            /*
            if (client.Count >= 2)
            {
                serverMessage = newClient.connectionID + ":Server is full";
            }
          
            else
            {
               

            }
            */
        }

        public static void SelectUsername(string serverMessage, int connectionID)
        {
            client[connectionID].username = serverMessage.Substring(0, serverMessage.Length - 28);
        }


        static int spawnNumRed = 11;
        static int spawnNumBlue = 21;
        public static void ChooseSpawnPoint(int connectionID, int packetID)
        {
            if (packetID == 0)
            {
                client[connectionID].socket.Close();
            }

            //Red Spawns
            if (packetID < 14)
            {
                Console.WriteLine(client[connectionID].username + " spawns at " + spawnNumRed);
                client[connectionID].packetID = spawnNumRed;
                InstantiatePlayer(connectionID);
                spawnNumRed++;
                if (spawnNumRed == 14)
                {
                    spawnNumRed = 11;
                }
            }

            //Blue Spawns
            else
            {
                Console.WriteLine(client[connectionID].username + " spawns at " + spawnNumBlue);
                client[connectionID].packetID = spawnNumBlue;
                InstantiatePlayer(connectionID);
                spawnNumBlue++;
                if (spawnNumBlue == 24)
                {
                    spawnNumBlue = 21;
                }
            }

        }
        //connectionID = new Player ID | packetID = Spawnpoint | item.Key = all excisting players
        public static void InstantiatePlayer(int connectionID)
        {
            foreach (var item in client)
            {
                if (client.Count == 1)
                {
                    DataSender.SendSpawnLocation(item.Key, connectionID, client[connectionID].packetID);
                    item.Value.OnServerAlready = true;
                }

                else if (item.Value.OnServerAlready == false)
                {
                    //one player to much
                    foreach (var excPlayer in client)
                    {
                        DataSender.SendSpawnLocation(connectionID, excPlayer.Key, excPlayer.Value.packetID);
                        item.Value.OnServerAlready = true;
                    }
                }

                else if (item.Key != connectionID && item.Value.OnServerAlready == true)
                {
                    DataSender.SendSpawnLocation(item.Key, connectionID, client[connectionID].packetID);
                }
            }
        }

        public static void SendLocationOfClients(int connectionID, string msg)
        {
            foreach (var item in client)
            {
                if (item.Key != connectionID && item.Value.OnServerAlready == true)
                {
                    DataSender.SendLocationClient(item.Key, msg);
                }
            }
        }

        public static void DisconnectClient(int connectionID, string msg)
        {
            string[] splitedData = msg.Split(':');
            string username = splitedData[0];
            client.Remove(connectionID);


            //Send every remaining client a message, that one client has disconnected
            foreach (var item in client)
            {
                DataSender.SendClientDisconnect(item.Key, msg);
            }
        }

        public static void SpawnRocket(int connectionID, string msg)
        {
            //Send every Client that one Client has spawned a rocket
            foreach (var item in client)
            {
                if (item.Key != connectionID)
                {
                    DataSender.SendRocketHasBeenSpawned(item.Key, msg);
                }
            }
        }

        public static void DestroyPlayer(int connectionID, string msg)
        {
            //Send every Client that one Client died
            foreach (var item in client)
            {
                if (item.Key == connectionID)
                {
                    item.Value.OnServerAlready = false;
                }
                if (item.Key != connectionID)
                {
                    DataSender.SendPlayerDied(item.Key, msg);

                }
            }
        }

        public static void GivePlayerDamage(int connectionID, string msg)

        {
            string[] splitedData = msg.Split(':');
            string username = splitedData[0];

            foreach (var item in client)
            {
                if (item.Value.username == username)
                {
                    DataSender.SendPlayerDamage(item.Key, msg);
                }
            }
        }

        //If the Server wants to send a Message through the TCP Client,
        //It sends it through this Method
        public static void SendDataTo(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((data.GetUpperBound(0) - data.GetLowerBound(0)) + 1);
            buffer.WriteBytes(data);
            client[connectionID].stream.BeginWrite(buffer.ToArray(), 0, buffer.ToArray().Length, null, null);
            buffer.Dispose();
        }
    }
}
