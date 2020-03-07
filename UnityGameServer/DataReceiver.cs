using System;

namespace UnityGameServer
{

    //All Available Packets from the Client
    public enum ClientPackets
    {
        CHelloServer = 1,
        CChooseTeamRed = 10,
        CChooseTeamBlue = 20,
        CLocationClient = 2,
        CDisconnetClient = 3,
        CRocketHasBeenSpawned = 4,
        CPlayerDied = 5,
        COtherPlayerDamage = 6
        
    }
    class DataReceiver
    {
        //Handles the first Messsage from the Client
        public static void HandleHelloClient(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();

            Console.WriteLine("Client: " + msg);
            ClientManager.SelectUsername(msg, connectionID);
        }

        // Team Red = 1
        // Team Blue = 2
        public static void HandleChooseTeamRed(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();
            Console.WriteLine("Client: " + msg);

            ClientManager.ChooseSpawnPoint(connectionID, packetID);
        }
        public static void HandleChooseTeamBlue(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();
            Console.WriteLine("Client: " + msg);

            ClientManager.ChooseSpawnPoint(connectionID, packetID);
        }

        //not ready
        public static void HandleLocationClient(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();
            ClientManager.SendLocationOfClients(connectionID, msg);
        }
        public static void HandleDisconnectClient(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();
            Console.WriteLine(msg);
            ClientManager.DisconnectClient(connectionID, msg);
        }
        public static void HandleRocketHasBeenSpawned(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();
            Console.WriteLine(msg);
            ClientManager.SpawnRocket(connectionID, msg);
        }
        public static void HandlePlayerDied(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();
            Console.WriteLine(msg);
            ClientManager.DestroyPlayer(connectionID, msg);
        }

        public static void HandleGivePlayerDamage(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            string msg = buffer.ReadString();
            buffer.Dispose();
            Console.WriteLine(msg);
            ClientManager.GivePlayerDamage(connectionID, msg);
        }


    }
}
