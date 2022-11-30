using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Client
{
    internal class Chat
    {
        int id;
        List<String> utenti;
        String titolo = "";
        List<Messaggio> messaggi;
        String nome = MainWindow.getNome();

        public Chat(List<String> utenti,String titolo,int id)
        {
            this.utenti = utenti;
            this.titolo = titolo;
            messaggi = new List<Messaggio>();
            this.id = id;
        }

        public Chat(List<String> utenti,int id) {
            this.utenti = utenti;
            messaggi = new List<Messaggio>();
            this.id = id;
        }

        public Chat(List<String> utenti, int id)
        {
            this.utenti = utenti;
            this.id = id;
        }

        public String toString()
        {
            if (titolo == "") {
                for (int i = 0; i < utenti.Count; i++)
                    if (utenti[i] != nome)
                        return utenti[i];
            }
            else 
                return titolo;
            return "";
        }

        public List<Messaggio> toChat(String s)
        {
            String[] mess = s.Split(';');
            for (int i = 1; i < mess.Length; i++)
            {
                
            }

        }

        public List<Chat> toList(String s)
        {
            String[] chat = s.Split(';');
            List<Chat> list = new List<Chat>();
            if (s[1] == 'g')
            {
            }
            else if (s[1] == 's')
            {
                if (checkChat(chat))
                    list.Add(new Chat())
            }

        }

        public bool checkChat(String[] chat)
        {
            bool b = false;
            for (int i = 1; i < chat.Length-1; i++)
            {
                if (chat[i] == nome)
                    b = true;
            }
            return b;
        }
    }
}
