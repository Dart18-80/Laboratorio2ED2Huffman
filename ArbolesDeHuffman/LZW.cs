using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArbolesDeHuffman
{
    public class LZW 
    {
        Dictionary<string, int> Diccionario;
        Dictionary<int, string> Diccionariosalida;
        string deco;


        public LZW()
        {
            Diccionario = new Dictionary<string, int>();
            Diccionariosalida = new Dictionary<int, string>();
            for (int i = 0; i < 256; i++)
            {
                Diccionario.Add(((char)i) + "", i);
                Diccionariosalida.Add(i, (char)i + "");
            }
        }

        public int Encode(string entrada, string encodificador)
        {
            if (Diccionario.ContainsKey(entrada))
            {
                return -1;
            }
            else
            {
               
                Diccionario.Add(entrada, Diccionario.Count);
                return Diccionario[encodificador];
            }
        }

        public void Fill(byte entrada)
        {
            string insert = "" + (char)entrada;
            if (!Diccionario.ContainsKey(insert))
            {
                Diccionario.Add(insert, Diccionario.Count);
            }
        }

        public string Firstdeco(int entrada)
        {
            deco = Diccionariosalida[entrada];
            return Diccionariosalida[entrada];
        }

        public string Decode(int entrada)
        {
            string resultado = null;

            if (Diccionariosalida.ContainsKey(entrada))
            {
                resultado = Diccionariosalida[entrada];
            }
            else if (entrada == Diccionariosalida.Count)
                resultado = deco + deco[0];


            Diccionariosalida.Add(Diccionariosalida.Count, deco + resultado[0]);

            deco = resultado;
            return resultado;
        }
    }
}
