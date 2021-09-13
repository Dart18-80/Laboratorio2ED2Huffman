using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbolesDeHuffman;

namespace Laboratorio2ED2.Models
{
    public class Letras : Huffman<Letras>, IComparable
    {
        public double index { get; set; }
        public string Letra { get; set; }
        public string Binario { get; set; }

        public int CompareToIndices(Letras x, Letras y) 
        {
            int Comparar = x.index.CompareTo(y.index);
            if (Comparar == 0)
                return 0;
            else if (Comparar < 0)
                return -1;
            else
                return 1;
        }

        public int CompareToimpresion(Letras x, char y)
        {
            if (x.Letra == Convert.ToString(y))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public int CompareToSalida(Letras x)
        {
            if (x.index > 0.8 && x.index < 1.2)
            {
                return 0;
            }
            else 
            {
                return 1;
            }
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public Letras SumaDeIndices(Letras a, Letras b)
        {
            Letras Nuevo = new Letras();
            Nuevo.index = a.index + b.index;
            Nuevo.Letra = null;
            return Nuevo; 
        }

        public Letras AsignarBinario(Letras b,string a)
        {
            b.Binario = a;
            return b;
        }
    }
}
