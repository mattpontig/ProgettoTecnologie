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
        public static List<Chat> toList(String s, String nome)
        {
            //"g/s,(titolo),nome,nome,nome..."

            String[] riga = s.Split(';');
            List<Chat> list = new List<Chat>();
            for (int i = 0; i < riga.Length; i++)
            {
                String[] chat = riga[i].Split(',');
                List<String> utente = new List<String>();
                if (s[0] == 'g')
                {
                    if (checkChat(chat, nome))
                    {
                        for (int j = 2; j < chat.Length; j++)
                        {
                             utente.Add(chat[j].ToString());

                        }
                        list.Add(new Chat(utente, chat[1].ToString()));
                    }
                }
                else if (s[0] == 's')
                {
                    if (checkChat(chat, nome))
                    {
                        utente.Add(s[1].ToString());
                        utente.Add(s[2].ToString());
                        list.Add(new Chat(utente));
                    }
                    

                }
            }
            return list;
        }


        public static bool checkChat(String[] chat, String nome)
        {
            bool b = false;
            for (int i = 0; i < chat.Length - 1; i++)
            {
                if (chat[i] == nome)
                    b = true;
            }
            return b;
        }
    }
}
