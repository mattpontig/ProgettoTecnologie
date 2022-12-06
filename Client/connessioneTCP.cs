using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    public class connessioneTCP
    {
        public static TcpClient client { get; set; }
        public Int32 port { get; set; }
        public String server = "172.0.0.1";
        public bool toClose = false;

        private static connessioneTCP _instance = null;
        public static connessioneTCP getInstance()
        {
            if (_instance == null)
                _instance = new connessioneTCP();

            return _instance;
        }
        private connessioneTCP() {}

        /*public connessioneTCP()
        {
            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer
            // connected to the same address as specified by the server, port
            // combination.
            port = 8080;
            client = new TcpClient(server, port);
        }*/

        Byte[] data;
        NetworkStream stream;
        public void send(String message)
        {
            stream = client.GetStream();
            try
            {

                // Translate the passed message into ASCII and store it as a Byte array.
                data = System.Text.Encoding.UTF8.GetBytes(message + "\r\nEND");

                // Get a client stream for reading and writing.
                //Stream stream = client.GetStream();
                // Send the message to the connected TcpServer.
                stream.Write(data, 0, data.Length);
                stream.Flush();

                Console.WriteLine("Sent: {0}", message);
                
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

        }

        public String recive()
        {
            stream = client.GetStream();
            // String to store the response ASCII representation.
            String responseData = String.Empty;
            try
            {

                // Get a client stream for reading and writing.
                //stream = client.GetStream();

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                stream.Flush();
                responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
            return responseData;
        }


        public TcpClient getSocket()
        {
            return client;
        }

        public void setSocket(TcpClient socket)
        {
            client = socket;
        }
    }

}

