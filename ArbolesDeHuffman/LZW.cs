using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolesDeHuffman
{
    public class LZW <T> where T : IComparable
    {
        public void CrearDiccionario(string texto) 
        {
            char[] ArrayTexto = texto.ToCharArray();
            char[] Diccionario = ArrayTexto.Distinct().ToArray();
            int LongitudInicial = Diccionario.Length;
            Array.Sort(Diccionario);

            while (true)
            {
                 
            }
        }

        public bool Verificar(char[] Diccionario, string Comparar) 
        {
            for (int i = 0; i < Diccionario.Length; i++)
            {
                if (Diccionario[i].ToString() == Comparar)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
