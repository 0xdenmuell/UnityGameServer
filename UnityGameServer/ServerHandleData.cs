using System;
using System.Collections.Generic;
using System.Text;

namespace UnityGameServer
{
    static class ServerHandleData
    {
        public delegate void Packet(int connectionID, byte[] data);
        public static Dictionary<int, Packet> packets = new Dictionary<int, Packet>();

        //All Availble Client Packets
        public static void InitializePackets()
        {
            packets.Add((int)ClientPackets.CHelloServer, DataReceiver.HandleHelloClient);
            packets.Add((int)ClientPackets.CChooseTeamRed, DataReceiver.HandleChooseTeamRed);
            packets.Add((int)ClientPackets.CChooseTeamBlue, DataReceiver.HandleChooseTeamBlue);
            packets.Add((int)ClientPackets.CLocationClient, DataReceiver.HandleLocationClient);
            packets.Add((int)ClientPackets.CDisconnetClient, DataReceiver.HandleDisconnectClient);
            packets.Add((int)ClientPackets.CRocketHasBeenSpawned, DataReceiver.HandleRocketHasBeenSpawned);
            packets.Add((int)ClientPackets.CPlayerDied, DataReceiver.HandlePlayerDied);
            packets.Add((int)ClientPackets.COtherPlayerDamage, DataReceiver.HandleGivePlayerDamage);
        }

        //Checks if the transmission is in order
        public static void HandleData(int connectionID, byte[] data)
        {
            byte[] buffer = (byte[])data.Clone();
            int pLength = 0;

            //If the Saved client in ClientManager is null, create a new Buffer,
            if (ClientManager.client[connectionID].buffer == null)
            {
                ClientManager.client[connectionID].buffer = new ByteBuffer();
            }

            ClientManager.client[connectionID].buffer.WriteBytes(buffer);

            //If the Saved data in client in ClientManager is 0, create a new Buffer 
            if (ClientManager.client[connectionID].buffer.Count() == 0)
            {
                ClientManager.client[connectionID].buffer.Clear();
                return;
            }

            if (ClientManager.client[connectionID].buffer.Length() >= 4)
            {
                pLength = ClientManager.client[connectionID].buffer.ReadInteger(false);
                if (pLength <= 0)
                {
                    ClientManager.client[connectionID].buffer.Clear();
                    return;
                }
            }

            while (pLength > 0 & pLength <= ClientManager.client[connectionID].buffer.Length() - 4)
            {
                if (pLength <= ClientManager.client[connectionID].buffer.Length() - 4)
                {
                    ClientManager.client[connectionID].buffer.ReadInteger();
                    data = ClientManager.client[connectionID].buffer.ReadBytes(pLength);
                    HandleDataPackets(connectionID, data);
                }

                if (!(ClientManager.client.ContainsKey(connectionID)))
                {
                    return;
                }

                pLength = 0;
                if (ClientManager.client[connectionID].buffer.Length() >= 4)
                {
                    pLength = ClientManager.client[connectionID].buffer.ReadInteger(false);
                    if (pLength <= 0)
                    {
                        ClientManager.client[connectionID].buffer.Clear();
                        return;
                    }
                }
            }
            if (pLength <= 1)
            {
                ClientManager.client[connectionID].buffer.Clear();
            }
        }

        //Checks which action has to be done and which packet has arrived
        private static void HandleDataPackets(int connectionID, byte[] data)
        {
            ByteBuffer buffer = new ByteBuffer();
            buffer.WriteBytes(data);
            int packetID = buffer.ReadInteger();
            buffer.Dispose();
            if (packets.TryGetValue(packetID, out Packet packet))
            {
                packet.Invoke(connectionID, data);
            }
        }
    }
}