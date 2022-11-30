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
        //int id;
        List<String> utenti;
        String titolo = "";
        List<Messaggio> messaggi;
        public String nome = MainWindow.getNome();
        public bool chatCaricata { get; set; }

        public Chat(List<String> utenti, String titolo)
        {
            this.utenti = utenti;
            this.titolo = titolo;
            chatCaricata = false;
            //this.id = id;
        }

        public Chat(List<String> utenti)
        {
            this.utenti = utenti;
            chatCaricata = false;
            //this.id = id;
        }

        public String toString()
        {
            if (titolo == "")
            {
                for (int i = 0; i < utenti.Count; i++)
                    if (utenti[i] != nome)
                        return utenti[i];
            }
            else
                return titolo;
            return "";
        }

        /*public List<Messaggio> toChat(String s)
        {
            String[] mess = s.Split(';');
            for (int i = 1; i < mess.Length; i++)
            {
                
            }

        }*/
    }
}
