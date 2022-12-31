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
    public class ClientSocket
    {
        private int PORT;
        public TcpClient socket;
        NetworkStream stream;
        public String m;
        
        public ClientSocket(int port)
        {
            this.PORT = port;
            //socket = new TcpClient(addr, PORT);
            socket = new TcpClient();
            socket.Connect("localhost", port);
            stream = socket.GetStream();

            connessioneTCP inst = connessioneTCP.getInstance();
            inst.setSocket(socket, stream);
        }

        public void run()
        {
            connessioneTCP inst = connessioneTCP.getInstance();
            try
            {
                //inst.setSocket(socket,stream);
                while (true)
                {
                    try
                    {
                        String line = inst.recive();
                        line.Trim();
                        m = "";
                        m = line;
                        Console.WriteLine("Ricevuto dal server: " + line);
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

        public NetworkStream getStream()
        {
            return stream;
        }

    }
}