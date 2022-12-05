using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace Client
{
    internal class ClientSocket
    {
        private int PORT;
        public TcpClient socket;
        NetworkStream stream;
        
        public ClientSocket(int port)
        {
            this.PORT = port;
            //socket = new TcpClient(addr, PORT);
            socket = new TcpClient();
            socket.Connect("localhost", port);
            stream = socket.GetStream();
        }

        public void run()
        {
            connessioneTCP inst = connessioneTCP.getInstance();

            try
            {
                inst.setSocket(socket);
                while (true)
                {
                    try
                    {
                        String line = inst.recive();
                        Console.WriteLine("Ricevuto dall'altro client: " + line);
                    }
                    catch (IOException e)
                    {

                    }

                }


            }
            catch (IOException e)
            {
                inst.toClose = true;  //impossibile prendere lo stream di input
                return;
            }
        }

        private NetworkStream getStream()
        {
            return stream;
        }

    }
}