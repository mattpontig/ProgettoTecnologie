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
        //;
        //2,g,primoGruppo,prova-triplo&32;
        //7,g,ProvaCreaGruppo,prova-ciao&31;
        //1,s,prova- non letto&29;
        //3,s,prova2-rompo tutto&28

        public static List<Chat> toList(String nome, String s)
        {
            //"id,g/s,(titolo),nome,nome,nome..."
                String[] riga = s.Split(';');
                List<Chat> list = new List<Chat>();
            try
            {
                for (int i = 2; i < riga.Length; i++)
                {
                    if (riga[i] != "")
                    {
                        String[] chat = riga[i].Split(',');
                        List<String> utente = new List<String>();
                        if (chat[1] == "g")
                        {
                            /*if (checkChat(chat, nome))
                            {*/
                            /*for (int j = 3; j < chat.Length; j++)
                            {
                                if (chat[j].ToString() != nome)
                                    utente.Add(chat[j].ToString());
                            }*/
                            //List<String> utenti, String titolo, int id,int ultiMess, String UltimoMess
                            String[] mess = (chat[3].Split('-'))[1].Split('&');
                            list.Add(new Chat(utente, chat[2].ToString(), int.Parse(chat[0].ToString()), int.Parse(mess[1]), mess[0]));
                            //}
                        }
                        else if (chat[1] == "s")
                        {
                            /*if (checkChat(chat, nome))
                            {*/
                            //"prova- non letto&29"
                            /*if (chat[3].ToString() != nome)
                                utente.Add(chat[3].ToString());
                            else
                                utente.Add(chat[4].ToString());*/

                            String[] div = chat[2].Split('-');
                            utente.Add(div[0]);
                            String[] mess = div[1].Split('&');
                            list.Add(new Chat(utente, "", int.Parse(chat[0].ToString()), int.Parse(mess[1]), mess[0]));
                            //}
                        }
                    }
                }
            }catch(Exception e) { 
            }
            list = bubbleSortChats(list);
            return list;
        }

        public static List<Chat> bubbleSortChats(List<Chat> arr)
        {
            List<Chat> l = arr;
            int i, j;
            for (i = 0; i < l.Count; i++)
                // Last i elements are already in place
                for (j = 0; j < l.Count - i - 1; j++)
                    if (l[j].idUltimoMess < l[j + 1].idUltimoMess)
                    {
                        Chat tempswap = l[j];
                        l[j] = l[j + 1];
                        l[j + 1] = tempswap;
                    }
            return l;
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
            for (int i = 0; i < chat.Length-1; i++)
            {
                String[] mess = chat[i].ToString().Split(',');
                messaggi.Add(new Messaggio(int.Parse(mess[0]),mess[1], mess[2]));
            }
            messaggi = bubbleSortChat(messaggi);
            return messaggi;
        }

        public static List<Messaggio> bubbleSortChat(List<Messaggio> arr)
        {
            List<Messaggio> l = arr;
            int i, j;
            for (i = 0; i < l.Count; i++)

                // Last i elements are already in place
                for (j = 0; j < l.Count - i - 1; j++)
                    if (l[j].id > l[j + 1].id)
                    {
                        Messaggio tempswap = l[j];
                        l[j] = l[j + 1];
                        l[j + 1] = tempswap;
                    }
            return l;
        }

        public static List<Utente> toUser(String s)
        {
            List<Utente> utente = new List<Utente>();
            String[] chat = s.Split(';');
            for (int i = 0; i < chat.Length - 1; i++)
            {
                String[] ut = chat[i].ToString().Split(',');
                utente.Add(new Utente(int.Parse(ut[0]), ut[1]));
            }
            return utente;
        }
    }
}
