using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Messaggio
    {
        String nome,messaggio;

        public Messaggio(String nome,String messaggio) { 
            this.nome = nome;
            this.messaggio = messaggio;
        }
    }
}
