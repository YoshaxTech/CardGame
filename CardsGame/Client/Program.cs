using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the server IP and port in the format 192.168.0.1:10000 and press return:");
            string serverInfo = Console.ReadLine();

            string serverIP = serverInfo.Split(':').First();
            int serverPort = int.Parse(serverInfo.Split(':').Last());


            NetworkComms.AppendGlobalIncomingPacketHandler<string>("Message", PrintIncomingMessage);
            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));
            foreach (System.Net.IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
                Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);

            int loopCounter = 1;
            while (true)
            {
                string messageToSend = "This is message #" + loopCounter;
                Console.WriteLine("Sending message to server saying '" + messageToSend + "'");

                //NetworkComms.SendObject("Message", serverIP, serverPort, messageToSend);

                Console.WriteLine("\nPress q to quit or any other key to send another message.");
                if (Console.ReadKey(true).Key == ConsoleKey.Q) break;
                else loopCounter++;
            }

            NetworkComms.Shutdown();
        }
        private static void PrintIncomingMessage(PacketHeader header, Connection connection, string message)
        {
            Console.WriteLine("\nA message was received from " + connection.ToString() + " which said '" + message + "'.");
        }
    }
}
