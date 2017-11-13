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
        static CardPlayed cardPlayed = new CardPlayed();
        //static MessageObject msg;
        static int        nbPlayers = 0;
        static  int i = 0;
        static bool end = false;
        static bool play = false;
        static bool wait = false;
        static bool firstPlay = false;
        static bool secPlay = false;
        static int nplay = 1;
        //static string card1;
        //static string card2;
        static int point1 = 0;
        static int point2 = 0;
        static List<Card> deck = new List<Card>();
        private static Random rng = new Random();
        static int nturn = 0;
        static void Main(string[] args)
        {
            NetworkComms.AppendGlobalIncomingPacketHandler<MessageObject>("Message", Connected);
            Connection.StartListening(ConnectionType.TCP, new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0));

            Console.WriteLine("Server listening for TCP connection on:");
            foreach (System.Net.IPEndPoint localEndPoint in Connection.ExistingLocalListenEndPoints(ConnectionType.TCP))
                    Console.WriteLine("{0}:{1}", localEndPoint.Address, localEndPoint.Port);

            createDeck();
            /*
            int i = 0;
            while (i < 52)
            {
                Console.WriteLine("[" + deck[i].rank + " of " + deck[i].sign + "]");
                i++;
            }
            i = 0;
            Console.WriteLine("\n////////////////////////////////\n");
            */
            Shuffle();
            /*
            while (i < 52)
            {
                Console.WriteLine("[" + deck[i].rank + " of " + deck[i].sign + "]");
                i++;
            }
            */
            bool cond = true;

            while (end != true)
            {
                if (play == true)
                {
                    play = false;
                    MessageObject msg = new MessageObject("Bienvenue", "Bienvenue");
                    if (nturn >= 52)
                    {
                        msg.action = "End";
                        if (point1 > point2)
                        {
                            msg.action = "End";
                            msg.message = "Vous avez gagné !";
                            player1.SendObject("Message", msg);
                            msg.action = "End";
                            msg.message = "Vous avez perdu...";
                            player2.SendObject("Message", msg);
                        }
                        else if (point2 > point1)
                        {
                            msg.action = "End";
                            msg.message = "Vous avez gagné !";
                            player2.SendObject("Message", msg);
                            msg.action = "End";
                            msg.message = "Vous avez perdu...";
                            player1.SendObject("Message", msg);
                        }
                        else if (point1 == point2)
                        {
                            msg.action = "End";
                            msg.message = "Egalité !";
                            player1.SendObject("Message", msg);
                            player2.SendObject("Message", msg);
                        }
                        end = true;
                    }
                    else
                    {

                        if (nplay == 1)
                        {
                            msg.action = "You";
                            if (nturn != 0)
                            {
                                msg.message = "L'adversaire a joué la carte : " + cardPlayed.card2 + "\nA vous de poser une carte...";
                            }
                            else
                            {
                                msg.message = "A vous de poser une carte...";
                            }
                            player1.SendObject("Message", msg);
                            msg.action = "Other";
                            msg.message = "L'adversaire pose une carte...";
                            player2.SendObject("Message", msg);
                            nturn++;
                        }
                        else
                        {
                            msg.action = "You";
                            msg.message = "L'adversaire a joué la carte : " + cardPlayed.card1 + "\nA vous de poser une carte...";
                            player2.SendObject("Message", msg);
                            msg.action = "Other";
                            msg.message = "L'adversaire pose une carte...";
                            player1.SendObject("Message", msg);
                            nturn++;
                            while (wait != true)
                            {
                                i++;
                            }
                            if (firstPlay == true && secPlay == true)
                            {
                                check_better();
                            }
                        }
                    }
                }

                if (nbPlayers == 2)
                {
                    if (cond == true)
                    {
                        cond = false;
                        MessageObject msg = new MessageObject("Bienvenue", "Bienvenue");
                        
                        player1.SendObject("Message", msg);
                        player2.SendObject("Message", msg);
                        msg.action = "Card";
                        int i = 0;
                        while (i < 26)
                        {
                            msg.message = "[" + deck[i].rank + " of " + deck[i].sign + "]";
                            player1.SendObject("Message", msg);
                            i++;
                        }
                        while (i < 52)
                        {
                            msg.message = "[" + deck[i].rank + " of " + deck[i].sign + "]";
                            player2.SendObject("Message", msg);
                            i++;
                        }
                        //Console.WriteLine("test\n");
                        msg.action = "Message";
                        msg.message = "Le jeu peut commencer.";
                        player1.SendObject("Message", msg);
                        player2.SendObject("Message", msg);
                        play = true;
                        //end = true;
                    }
                }
            }
            Console.WriteLine("\nPress any key to close server.");
            Console.ReadKey(true);
            NetworkComms.Shutdown();
        }

        private static void Connected(PacketHeader header, Connection connection, MessageObject receive)
        {
            
            if (Equals(receive.message, "Bonjours") == true) {
                nbPlayers++;
                
                if (nbPlayers == 1)
                {
                    
                    player1 = connection;
                }
                if (nbPlayers == 2)
                {
                    
                    player2 = connection;
                }
            }
            else if (Equals(receive.action, "card") == true)
            {
                if (nplay == 1)
                {
                    wait = true;
                    Console.WriteLine("LA CARTE RECU EST : " + receive.message);
                    cardPlayed.card1 = receive.message;
                    firstPlay = true;
                    nplay = 2;
                }
                else if (nplay == 2)
                {
                    wait = true;
                    Console.WriteLine("LA CARTE RECU EST : " + receive.message);
                    cardPlayed.card2 = receive.message;
                    secPlay = true;
                    nplay = 1;
                }
                play = true;
            }

            //Console.WriteLine("\nA message was received from " + connection.ToString() + " which said '" + receive.message + "'.");
            //connection.SendObject("Message", "Bonjour");
        }

        public static void createDeck()
        {
            fill_sign("Heart");
            fill_sign("Club");
            fill_sign("Diamond");
            fill_sign("Spades");
        }

        public static void fill_sign(String sign)
        {
            fill_rank(sign, "Two");
            fill_rank(sign, "Three");
            fill_rank(sign, "Four");
            fill_rank(sign, "Five");
            fill_rank(sign, "Six");
            fill_rank(sign, "Seven");
            fill_rank(sign, "Eight");
            fill_rank(sign, "Nine");
            fill_rank(sign, "Ten");
            fill_rank(sign, "Jack");
            fill_rank(sign, "Queen");
            fill_rank(sign, "King");
            fill_rank(sign, "Ace");
        }

        public static void fill_rank(String sign, String rank)
        {
            Card tmp = new Card();
            tmp.rank = rank;
            tmp.sign = sign;
            deck.Add(tmp);
        }

        public static void Shuffle()
        {
            Card tmp = new Card();
            for (int i = deck.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                tmp = deck[i];
                deck[i] = deck[j];
                deck[j] = tmp;
            }
        }

        public static string get_card(string str)
        {
            int n = 0;
            string dest ="";

            Console.WriteLine(str);
            while (str[n] != ' ' && str[n] != '\0')
            {
                dest = dest + str[n];
                n++;
            }
            return dest;
        }

        public static void check_better()
        {

            string rank1;
            string rank2;
            int i = 0;
            while (wait != true) { i++; }
            rank1 = get_card(cardPlayed.card1);
            rank2 = get_card(cardPlayed.card2);
            if (Equals(rank1, rank2) == false)
            {
               if (Equals(rank1, "[Ace") == true)
                {
                    point1++;
                }
               else if (Equals(rank2, "[Ace") == true)
                {
                    point2++;
                }
               else if (Equals(rank1, "[King") == true)
                {
                    point1++;
                }
               else if (Equals(rank2, "[King") == true)
                {
                    point2++;
                }
                else if (Equals(rank1, "[Queen") == true)
                {
                    point1++;
                }
                else if (Equals(rank2, "[Queen") == true)
                {
                    point2++;
                }
                else if (Equals(rank1, "[Jack") == true)
                {
                    point1++;
                }
                else if (Equals(rank2, "[Jack") == true)
                {
                    point2++;
                }
                else if (Equals(rank1, "[Ten") == true)
                {
                    point1++;
                }
                else if (Equals(rank2, "[Ten") == true)
                {
                    point2++;
                }
                else if (Equals(rank1, "[Nine") == true)
                {
                    point1++;
                }
                else if (Equals(rank2, "[Nine") == true)
                {
                    point2++;
                }
                else if (Equals(rank1, "[Eight") == true)
                {
                    point1++;
                }
                else if (Equals(rank2, "[Eight") == true)
                {
                    point2++;
                }
                else if (Equals(rank1, "[Seven") == true)
                {
                    point1++;
                }
                else if (Equals(rank2, "[Seven") == true)
                {
                    point2++;
                }
                else if (Equals(rank1, "[Six") == true)
                {
                    point1++;
                }
                else if (Equals(rank2, "[Six") == true)
                {
                    point2++;
                }
                else if (Equals(rank1, "[Five") == true)
                {
                    point1++;
                }
                else if (Equals(rank2, "[Five") == true)
                {
                    point2++;
                }
                else if (Equals(rank1, "[Four") == true)
                {
                    point1++;
                }
                else if (Equals(rank2, "[Four") == true)
                {
                    point2++;
                }
                else if (Equals(rank1, "[Three") == true)
                {
                    point1++;
                }
                else if (Equals(rank2, "[Three") == true)
                {
                    point2++;
                }
                else if (Equals(rank1, "[Two") == true)
                {
                    point1++;
                }
                else if (Equals(rank2, "[Two") == true)
                {
                    point2++;
                }
            }
        }
    }
}
