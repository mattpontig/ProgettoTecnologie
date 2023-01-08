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
        public List<String> utenti { get; set; }
        public String titolo { get; set; }
        public List<Messaggio> messaggi { get; set; }
        public String nome = MainWindow.getNome();
        public bool chatCaricata { get; set; }
        public int messNonLetti { get; set; }
        public int idUltimoMess { get; set; }
        public String UltimoMess { get; set; }

        public Chat()
        {
            this.utenti = new List<String>();
            this.titolo = "";
            chatCaricata = false;
            messaggi = null;
            this.id = 0;
            messNonLetti = 0;
            idUltimoMess = 0;
            UltimoMess = "";
        }

        public Chat(List<String> utenti, String titolo, int id,int ultiMess,String UltimoMess,int messNonLetti)
        {
            this.utenti = utenti;
            this.titolo = titolo;
            chatCaricata = false;
            messaggi = null;
            this.id = id;
            this.messNonLetti = messNonLetti;
            idUltimoMess = ultiMess;
            this.UltimoMess = UltimoMess;
        }

        public Chat(List<String> utenti,int id)
        {
            this.utenti = utenti;
            chatCaricata = false;
            messaggi = null;
            this.id = id;
            messNonLetti = 0;
            idUltimoMess = 0;
            UltimoMess = "";
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
            if (messNonLetti != 0)
                den += "\t\t" + messNonLetti;
            return den;
        }
        public String getName()
        {
            String den = "";
            if (titolo == "")
            {
                for (int i = 0; i < utenti.Count; i++)
                    if (utenti[i] != nome)
                        den = utenti[i];
            }
            else
                den = titolo;
            return den;
        }
    }
}
