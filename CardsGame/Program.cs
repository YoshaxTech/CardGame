using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Server
{
    class Program
    {
        static Connection player1;
        static Connection player2;
        static Connection player3;
        static Connection player4;

        static void Main(string[] args)
        {
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", PrintIncomingMessage);
            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));

            Console.WriteLine("Server listening for TCP connection on:");
            foreach (System.Net.IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
                    Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);
            Console.WriteLine("\nPress any key to close server.");
            Console.ReadKey(true);

            NetworkComms.Shutdown();
        }

        private static void PrintIncomingMessage(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine("\nA message was received from " + connection.ToString() + " which said '" + message + "'.");
            connection.SendObject("Message", "Bonjour");
        }
    }
}
