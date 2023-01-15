using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Messaggio
    {
        public String nome, messaggio;
        public int id;
        public int file = 0;

        public Messaggio(int id,String nome, String messaggio, int file)
        {
            this.id = id;
            this.nome = nome;
            this.messaggio = messaggio;
            this.file = file;
        }

        public String toMessHost()
        {
            return "you" + ":" + "\t" + messaggio;
        }

        public String toMessGuest()
        {
            return nome + ":" + "\t" + messaggio;
        }
    }
}
