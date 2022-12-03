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
        public int id { get; set; }
        List<String> utenti;
        String titolo = "";
        public List<Messaggio> messaggi { get; set; }
        public String nome = MainWindow.getNome();
        public bool chatCaricata { get; set; }

        public Chat(List<String> utenti, String titolo, int id)
        {
            this.utenti = utenti;
            this.titolo = titolo;
            chatCaricata = false;
            messaggi = new List<Messaggio>();
            this.id = id;
        }

        public Chat(List<String> utenti,int id)
        {
            this.utenti = utenti;
            chatCaricata = false;
            messaggi = new List<Messaggio>();
            this.id = id;
        }

        public String toString()
        {
            String den = "";
            if (titolo == "")
            {
                for (int i = 0; i < utenti.Count; i++)
                    if (utenti[i] != nome)
                        den = utenti[i];
            }
            else
                den =  titolo;
            return den;
        }
    }
}
