using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbolesDeHuffman;

namespace Laboratorio2ED2.Models
{
    public class Texto : ILZW<Texto>, IComparable
    {
        public string Archivo { get; set ; }

        public int CompareToLetras(string Letra1, string Letra2)
        {
            int Comparar = Letra1.CompareTo(Letra2);
            if (Comparar == 0)
                return 0;
            else if (Comparar < 0)
                return -1;
            else
                return 1;

            throw new NotImplementedException();
        }
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
