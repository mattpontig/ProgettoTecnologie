using System;
using System.Collections;
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
            while (stream.CanWrite == false) { }
            try
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                data = System.Text.Encoding.Default.GetBytes(message + "\r\nEND\r\n");

                // Get a client stream for reading and writing.
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
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public String recive()
        {
            // String to store the response ASCII representation.
            String responseData = "";
            while (stream.CanRead == false || stream.DataAvailable == false) { }
            try
            {
                while (stream.DataAvailable == true)
                {
                    // Get a client stream for reading and writing.

                    // Receive the TcpServer.response.

                    // Buffer to store the response bytes.
                    data = new Byte[256];

                    // Read the first batch of the TcpServer response bytes.
                    Int32 bytes = stream.Read(data, 0, data.Length);
                    //bytes = stream.ReadAsync(data, 0, data.Length).Result;
                    responseData += System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                }
                //Console.WriteLine("Received: {0}", responseData);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return responseData;
        }

        public void sendFile(String fileName)
        {
            /*Socket socket = client.Client;


            while (stream.CanWrite == false) { }
            try
            {

                byte[] fileBytes = File.ReadAllBytes(fileName);

                // Get a client stream for reading and writing.
                // Send the message to the connected TcpServer.
                stream.Write(fileBytes, 0, fileBytes.Length);
                stream.Flush();

            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }*/
            BinaryWriter writer = new BinaryWriter(stream);
            byte[] file = File.ReadAllBytes(fileName);
            writer.Write(file);

        }

        public void reciveFile(String name)
        {

                byte[] buffer = new byte[4096];
                int bytesRead;

                try
                {
                    using (FileStream fileStream = new FileStream(name, FileMode.Create))
                    {
                        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            fileStream.Write(buffer, 0, bytesRead);
                        }

                    }
                }
                catch (Exception e) { }
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

