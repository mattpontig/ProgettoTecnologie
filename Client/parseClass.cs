using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Client
{
    internal class parseClass
    {
        public static List<Chat> toList(String nome, String s)
        {
            //"id,g/s,(titolo),nome,nome,nome..."
                String[] riga = s.Split(';');
                List<Chat> list = new List<Chat>();
            try
            {
                for (int i = 0; i < riga.Length - 1; i++)
                {
                    String[] chat = riga[i].Split(',');
                    List<String> utente = new List<String>();
                    if (chat[1] == "g")
                    {
                        /*if (checkChat(chat, nome))
                        {*/
                        for (int j = 3; j < chat.Length; j++)
                        {
                            if (chat[j].ToString() != nome)
                                utente.Add(chat[j].ToString());
                        }
                        list.Add(new Chat(utente, chat[2].ToString(), int.Parse(chat[0].ToString())));
                        //}
                    }
                    else if (chat[1] == "s")
                    {
                        /*if (checkChat(chat, nome))
                        {*/
                        if (chat[3].ToString() != nome)
                            utente.Add(chat[3].ToString());
                        else
                            utente.Add(chat[4].ToString());
                        list.Add(new Chat(utente, int.Parse(chat[0].ToString())));
                        //}
                    }
                }
            }catch(Exception e) { 
            }
            return list;
        }


        /*public static bool checkChat(String[] chat, String nome)
        {
            bool b = false;
            for (int i = 0; i < chat.Length - 1; i++)
            {
                if (chat[i] == nome)
                    b = true;
            }
            return b;
        }*/

        public static List<Messaggio> toChat(String s)
        {
            List<Messaggio> messaggi = new List<Messaggio>();
            String[] chat = s.Split(';');
            for (int i = 1; i < chat.Length; i++)
            {
                String[] mess = chat[i].ToString().Split(',');
                messaggi.Add(new Messaggio(mess[0], mess[1]));
            }
            return messaggi;
        }
    }
}
