using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace gamehost
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Variables
            int[,] ttt = new int[2, 2]; // For the XO's :D
            Byte[] bytes = new Byte[256]; // Buffer for reading data
            String data = null;
            int i;
            #endregion

            TcpListener server = null;
            try
            {
                Int32 port = 13000; // Set the TcpListener on port 13000.
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                server = new TcpListener(localAddr, port); // TcpListener server = new TcpListener(port);
                server.Start(); // Start listening for client requests.            

                while (true) // Enter the listening loop.
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = server.AcceptTcpClient(); // Perform a blocking call to accept requests.
                    Console.WriteLine("Connected!");
                    data = null;
                    NetworkStream stream = client.GetStream(); // Get a stream object for reading and writing


                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0) // Loop to receive all the data sent by the client.
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i); // Translate data bytes to a ASCII string.
                        Console.WriteLine("Received: {0}", data);

                        data = data.ToUpper(); // Process the data sent by the client.
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        stream.Write(msg, 0, msg.Length); // Send back a response.
                        Console.WriteLine("Sent: {0}", data);
                    }

                    client.Close(); // Shutdown and end connection
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


        }
    }
}

