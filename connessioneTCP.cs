using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    public class connessioneTCP
    {
        public static TcpClient client { get; set; }
        public Int32 port { get; set; }
        public String server = "127.0.0.1";
        public bool toClose = false;
        Socket socket;

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
        NetworkStream stream = null;
        public void send(String message)
        {
            // Translate the passed message into ASCII and store it as a Byte array.
            //data = System.Text.Encoding.ASCII.GetBytes(message);

            // Get a client stream for reading and writing.
            //stream = client.GetStream();

            // Send the message to the connected TcpServer.
            //stream.Write(data, 0, data.Length);

            //Console.WriteLine("Sent: {0}", message);
            //stream = client.GetStream();
            try
            {
                using (client=new TcpClient(server, 8080))
                {
                    using (stream=client.GetStream())
                    {
                        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                        stream.Flush();
                    }
                }
                // Translate the passed message into ASCII and store it as a Byte array.
                //data = System.Text.Encoding.ASCII.GetBytes(message);
                // Translate the passed message into ASCII and store it as a Byte array.
                //data = System.Text.Encoding.UTF8.GetBytes(message + "\r\nEND");

                // Get a client stream for reading and writing.
                //Stream stream = client.GetStream();
                // Send the message to the connected TcpServer.
                // Get a client stream for reading and writing.
                //NetworkStream stream = client.GetStream();
                //stream.Write(data, 0, data.Length);
                //stream.Flush();

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

        public void sendImg(String path)
        {
            socket = client.Client;

            FileStream stream = File.OpenRead(path);

            /*int length = IPAddress.HostToNetworkOrder((int)stream.Length);
            socket.Send(BitConverter.GetBytes(length), SocketFlags.None);

            byte[] buffer = new byte[1024];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                socket.Send(buffer, 0, read, SocketFlags.None);
            }*/
            // send the file
            byte[] buffer = ReadImageFile(path);

            socket.Send(buffer, buffer.Length, SocketFlags.None);

        }

        public void reciveImg(String path)
        {
            socket = client.Client;

            FileStream stream = File.OpenRead(path);

            int length = IPAddress.HostToNetworkOrder((int)stream.Length);
            socket.Send(BitConverter.GetBytes(length), SocketFlags.None);

            byte[] buffer = new byte[1024];
            int read;
            while ((read = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                socket.Send(buffer, 0, read, SocketFlags.None);
            }
        }

        public String recive()
        {
            //NetworkStream stream = client.GetStream();
            // String to store the response ASCII representation.
            String responseData = String.Empty;
            try
            {
                using (client = new TcpClient(server, 8080))
                {
                    using (stream = client.GetStream())
                    {
                        Byte[] // Buffer to store the response bytes.
                data = new Byte[256];

                        // Read the first batch of the TcpServer response bytes.
                        Int32 bytes = stream.Read(data, 0, data.Length);
                        //stream.Flush();
                        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    }
                }
                // Get a client stream for reading and writing.
                //stream = client.GetStream();

                // Receive the TcpServer.response.

                
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

        private static byte[] ReadImageFile(String img)
        {
            FileInfo fileInfo = new FileInfo(img);
            byte[] buf = new byte[fileInfo.Length];
            FileStream fs = new FileStream(img, FileMode.Open, FileAccess.Read);
            fs.Read(buf, 0, buf.Length);
            fs.Close();
            //fileInfo.Delete ();
            GC.ReRegisterForFinalize(fileInfo);
            GC.ReRegisterForFinalize(fs);
            return buf;
        }
    }

}

