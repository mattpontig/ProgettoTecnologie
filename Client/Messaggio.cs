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

        public Messaggio(int id,String nome, String messaggio)
        {
            this.id = id;
            this.nome = nome;
            this.messaggio = messaggio;
        }

        public String toMess()
        {
            return nome + ":" + "\t" + messaggio;
        }
    }
}
