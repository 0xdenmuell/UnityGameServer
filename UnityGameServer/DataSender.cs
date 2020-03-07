using System;

namespace UnityGameServer
{
    public enum ServerPackets
    {
        SWelcomeMessage = 1,
        SChooseTeamRed1 = 11,
        SChooseTeamRed2 = 12,
        SChooseTeamRed3 = 13,
        SChooseTeamBlue1 = 21,
        SChooseTeamBlue2 = 22,
        SChooseTeamBlue3 = 23,
        SLocationClient = 2,
        SDisconnectClient = 3,
        SRocketHasBeenSpawned = 4,
        SPlayerDied = 5,
        SPlayerDamage = 6

    }
    static class DataSender
    {
        //Sends the first Welcome Message to the Client, to check if the connection is stable
        public static void SendWelcomeMessage(int connectionID)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ServerPackets.SWelcomeMessage);
            buffer.WriteString("Welcome to the Server!");
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendSpawnLocation(int clientID, int connectionID, int packetID)
        {
            ByteBuffer buffer = new ByteBuffer();

            switch (packetID)
            {
                case 11:
                    buffer.WriteInteger((int)ServerPackets.SChooseTeamRed1);
                    break;
                case 12:
                    buffer.WriteInteger((int)ServerPackets.SChooseTeamRed2);
                    break;
                case 13:
                    buffer.WriteInteger((int)ServerPackets.SChooseTeamRed3);
                    break;
                case 21:
                    buffer.WriteInteger((int)ServerPackets.SChooseTeamBlue1);
                    break;
                case 22:
                    buffer.WriteInteger((int)ServerPackets.SChooseTeamBlue2);
                    break;
                case 23:
                    buffer.WriteInteger((int)ServerPackets.SChooseTeamBlue3);
                    break;
                default:
                    Console.WriteLine("Wrong packetID");
                    break;
            }
            string msg = ClientManager.client[connectionID].username + ":spawns at Spawnpoint:" + packetID;
            buffer.WriteString(msg);
            ClientManager.SendDataTo(clientID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendLocationClient(int connectionID, string msg)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ServerPackets.SLocationClient);
            buffer.WriteString(msg);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendClientDisconnect(int connectionID, string msg)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ServerPackets.SDisconnectClient);
            buffer.WriteString(msg);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendRocketHasBeenSpawned(int connectionID, string msg)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ServerPackets.SRocketHasBeenSpawned);
            buffer.WriteString(msg);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }

        public static void SendPlayerDied(int connectionID, string msg)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ServerPackets.SPlayerDied);
            buffer.WriteString(msg);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }
        public static void SendPlayerDamage(int connectionID, string msg)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteInteger((int)ServerPackets.SPlayerDamage);
            buffer.WriteString(msg);
            ClientManager.SendDataTo(connectionID, buffer.ToArray());
            buffer.Dispose();
        }
    }
}
