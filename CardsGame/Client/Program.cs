using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using Server;

namespace Client
{
    
    class Program
    {
        static bool end = false;
        static bool play = false;
        static MessageObject messageSend = new MessageObject("Bonjours", "Bonjouurs");
        static List<String> hand = new List<String>();

        static void Main(string[] args)
        {
            Console.WriteLine("Please enter the server IP and port in the format 192.168.0.1:10000 and press return:");
            string serverInfo = Console.ReadLine();

            string serverIP = serverInfo.Split(':').First();
            int serverPort = int.Parse(serverInfo.Split(':').Last());


            NetworkComms.AppendGlobalIncomingPacketHandler<MessageObject>("Message", PrintIncomingMessage);
            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));
            foreach (System.Net.IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
                Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);

            int loopCounter = 1;

            Console.WriteLine("Vous êtes connecté au serveur.");
            // msg.message = "Bonjours";
            // msg.action = "Bonjours";

            NetworkComms.SendObject("Message", serverIP, serverPort, messageSend);


            while (end == false)
            {
                
                
                
                if (play == true)
                {
                    play = false;
                    string str = "";
                    while (Equals(str, "play") != true)
                    {
                        Console.WriteLine("Jouez une carte (écrivez 'play')");

                        str = Console.ReadLine();
                    }
                    if (Equals(str, "play") == true)
                    {
                        messageSend.action = "card";
                        messageSend.message = hand[0];
                        hand.RemoveAt(0);
                        Console.WriteLine("vous avez joué la carte : " + messageSend.message);
                        NetworkComms.SendObject("Message", serverIP, serverPort, messageSend);
                    }
                    //joue 1 carte
                    //envoi la carte
                }
                

            }
            Console.WriteLine("Press q for quit. <3");
            Console.ReadKey(true);
            NetworkComms.Shutdown();
        }
        private static void PrintIncomingMessage(PacketHeader header, Connection connection, MessageObject message)
        {
            
            //Console.WriteLine(message.message);
            if (Equals(message.action, "Card") == true)
            {
                //Console.WriteLine(message.message);
                hand.Add(message.message);
            }
            else if (Equals(message.action, "You") == true)
            {
                Console.WriteLine(message.message);
                play = true;
            }
            else if (Equals(message.action, "End") == true)
            {
                Console.WriteLine(message.message);
                end = true;
            }
            else if (Equals(message.action, "Message") == true)
            {
                Console.WriteLine(message.message);
            }
        }
    }
}
