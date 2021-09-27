using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolesDeHuffman
{
    public class LZW <T> where T : IComparable , ILZW
    {
        public void CrearDiccionario(T texto) 
        {
            char[] Diccionario = texto.Archivo.Distinct();
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
