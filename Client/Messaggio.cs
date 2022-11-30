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

        public Messaggio(String nome, String messaggio)
        {
            this.nome = nome;
            this.messaggio = messaggio;
        }

        public String toMessHost()
        {
            return messaggio + "\t" + nome;
        }

        public String toMessGuest()
        {
            return nome + "\t" + messaggio;
        }
    }
}
