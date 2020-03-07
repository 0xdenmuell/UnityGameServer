using System;
using System.Collections.Generic;
using System.Text;

namespace UnityGameServer
{
    static class General
    {
        //Starts the Server
        public static void InitializeServer()
        {
            ServerTCP.InititalizeNetwork();
            Console.WriteLine("Server has been started!");
        }
    }
}
