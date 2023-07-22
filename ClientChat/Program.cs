using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SimpleTcpClient
{
    class Program
    {
        private static bool asynchroJuzdziala = default;

        //static async Task Main(string[] args)
        //{
        //    Console.OutputEncoding = Encoding.UTF8;

        //    byte[] data = new byte[1024];

        //    string input, stringData;

        //    IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

        //    Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //    try
        //    {
        //        server.Connect(ipep);

        //    }
        //    catch (SocketException e)
        //    {
        //        Console.WriteLine("Unable to connect to server.");

        //        Console.WriteLine(e.ToString());

        //        return;
        //    }

        //    int recv = server.Receive(data);

        //    stringData = Encoding.UTF8.GetString(data, 0, recv);

        //    Console.WriteLine(stringData);

        //    while (true)
        //    {
        //        input = String.Empty;
        //        if (!asynchroJuzdziala)
        //        {
        //            await Task.Run(() =>
        //            {

        //                asynchroJuzdziala = true;
        //                input = Console.ReadLine();//await Console.In.ReadLineAsync();

        //                server.Send(Encoding.UTF8.GetBytes(input));
        //                Console.WriteLine("You: " + input);
        //                asynchroJuzdziala = false;

        //        });
        //        }
        //        await Task.Run(() =>
        //        {
        //            //przemianowanko
        //            data = new byte[1024];
        //            recv = server.Receive(data);
        //            stringData = Encoding.UTF8.GetString(data, 0, recv);
        //            byte[] utf8string = System.Text.Encoding.UTF8.GetBytes(stringData);
        //            Console.WriteLine("Server: " + stringData);
        //        });
        //        if (input == "exit")
        //                  break;

        //    }
        //    Console.WriteLine("Disconnecting from server...");
        //    server.Shutdown(SocketShutdown.Both);
        //    server.Close();

        //    Console.WriteLine("Disconnected!");

        //    Console.ReadLine();

        //}
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            byte[] data = new byte[1024];
            string input, stringData;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);

            using (Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                try
                {
                    server.Connect(ipep);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("Unable to connect to server.");
                    Console.WriteLine(e.ToString());
                    return;
                }

                // Odbieranie wiadomości od serwera w tle
                Task.Run(() =>
                {
                    while (true)
                    {
                        Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString());
                        int recv = server.Receive(data, SocketFlags.None);
                        stringData = Encoding.UTF8.GetString(data, 0, recv);
                        Console.WriteLine("Server: " + stringData);
                    }
                });

                // Pętla do wysyłania wiadomości do serwera
                while (true)
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString());
                    input = Console.ReadLine();

                    if (input == "exit")
                        break;

                    server.Send(Encoding.UTF8.GetBytes(input));
                    Console.WriteLine("You: " + input);
                }

                Console.WriteLine("Disconnecting from server...");
                server.Shutdown(SocketShutdown.Both);
                server.Close();

                Console.WriteLine("Disconnected!");
            }

        }
    }
}