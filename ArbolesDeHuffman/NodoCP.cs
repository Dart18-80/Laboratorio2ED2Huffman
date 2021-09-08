using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolesDeHuffman
{
    public class NodoCP <T> where T : IComparable
    {
        public T Data { get; set; }
        public NodoCP<T> Siguiente { get; set; }
    }
}
