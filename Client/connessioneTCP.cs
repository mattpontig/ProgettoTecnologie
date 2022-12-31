using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        public TcpClient client;
        //public Int32 port { get; set; }
        //public String server = "172.0.0.1";
        public bool toClose = false;


        private static connessioneTCP _instance = null;
        public static connessioneTCP getInstance()
        {
            if (_instance == null)
                _instance = new connessioneTCP();

            return _instance;
        }
        private connessioneTCP() { }

        Byte[] data;
        public void send(String message)
        {
            while (stream.CanWrite == false){ }
                try
                {
                    // Translate the passed message into ASCII and store it as a Byte array.
                    data = System.Text.Encoding.Default.GetBytes(message + "\r\nEND\r\n");

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
            // String to store the response ASCII representation.
            String responseData = String.Empty;
            while (stream.CanRead == false) { }
                try
                {

                    // Get a client stream for reading and writing.
                    //stream = client.GetStream();

                    // Receive the TcpServer.response.

                    // Buffer to store the response bytes.
                    data = new Byte[256];
                    stream.Flush();
                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.Default.GetString(data/*, 0, bytes*/);

                    //Console.WriteLine("Received: {0}", bytes);

                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("ArgumentNullException: {0}", e);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                catch (IOException e) { }
            return responseData;
        }

        NetworkStream stream;
        public void setSocket(TcpClient socket, NetworkStream stream)
        {
            this.client = socket;
            this.stream = stream;
        }


        public TcpClient getSocket()
        {
            return client;
        }
    }
}

