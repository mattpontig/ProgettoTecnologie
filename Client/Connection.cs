using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Client;

namespace banco_client
{
    internal class Connection
    {
        UdpClient client = new UdpClient();
        IPEndPoint riceveEP = new IPEndPoint(IPAddress.Any, 0);
        public static Boolean online;

        public void invia(String dati)
        {
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(dati);

                client.Send(data, data.Length, "localhost", 12345);

                online = true;
            }
            catch(Exception ex)
            {
                online = false;
            }
            //recive();
        }

        public String recive()
        {
            String risposta = "";
            try
            {
                while (risposta == "")
                {

                    //dentro riceveEP  troverete ( dopo la Receive ) l'indirizzo e la porta del mittente ( server ) 

                    byte[] dataReceived = client.Receive(ref riceveEP);

                    risposta = Encoding.ASCII.GetString(dataReceived);

                    //Console.WriteLine(risposta);
                }

                online = true;
            }
            catch (Exception ex)
            {
                online = false;
            }
            return risposta;
        }
    }
}
