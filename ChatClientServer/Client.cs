using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClientServer
{

    class Client
    {
        static void Main(string[] args)
        {
            ExecuteClient();
        }

        static void ExecuteClient()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[1];
            Console.WriteLine("ip==="+ipAddr);

            IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 8080);

            try
            {
                listener.Connect(localEndPoint);
                Console.WriteLine("Connected to Server........");

                while (true)
                {
                    string input;
                    Console.WriteLine("Client : ");
                    input = Console.ReadLine();

                    byte[] messageSent = Encoding.ASCII.GetBytes(input);
                    int byteSent = listener.Send(messageSent);

                    byte[] messageReceived = new byte[1024];
                    int byteRecv = listener.Receive(messageReceived);

                    string data;
                    data = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);
                    Console.WriteLine("Server : ");
                    Console.WriteLine(data);

                    if (data.IndexOf("bye") > -1)
                        break;
                }

                listener.Shutdown(SocketShutdown.Both);
                listener.Close();
            }
            catch
            {
                Console.WriteLine("Unbale to connect");
            }
        }
    }
}