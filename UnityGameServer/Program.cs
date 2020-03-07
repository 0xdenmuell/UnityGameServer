using System;
using System.Collections.Generic;
using System.Text;

namespace UnityGameServer
{
    class Program
    {
        //Starts the Program and directs to the Server Start
        static void Main(string[] args)
        { 
           
            General.InitializeServer();
            Console.ReadLine();
        }
    }
}
