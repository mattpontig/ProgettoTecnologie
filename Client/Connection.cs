using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace banco_client
{
    internal class Connection
    {
        UdpClient client = new UdpClient();
        IPEndPoint riceveEP = new IPEndPoint(IPAddress.Any, 0);

        public void invia(String dati)
        {
            byte[] data = Encoding.ASCII.GetBytes(dati);

            client.Send(data, data.Length, "localhost", 12345);

            //recive();
        }

        public String recive()
        {
            String risposta = "";
            while (risposta == "")
            {

                //dentro riceveEP  troverete ( dopo la Receive ) l'indirizzo e la porta del mittente ( server ) 

                byte[] dataReceived = client.Receive(ref riceveEP);

                risposta = Encoding.ASCII.GetString(dataReceived);

                //Console.WriteLine(risposta);
            }

            return risposta;
        }
    }
}
