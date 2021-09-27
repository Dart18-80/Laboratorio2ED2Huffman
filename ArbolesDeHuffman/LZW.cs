using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolesDeHuffman
{
    public class LZW <T> where T : IComparable 
    {
        public  string textonuevo = "";
        public  int numciclo = 0;
        public  string codificacion = "";
        public void CrearLZW(string texto, Delegate Comparacion) 
        {
            char[] ArrayTexto = texto.ToCharArray();//El texto completo en un array 
            char[] Diccionario = ArrayTexto.Distinct().ToArray();//El primer diccionario
            List<string> listaletras = new List<string>(Diccionario.Length);
            for (int i = 0; i < Diccionario.Length; i++)
            {
                listaletras.Add(Diccionario[i].ToString());
            }
            listaletras.Sort();
            textonuevo = ArrayTexto[0].ToString();
            CrearDiccionario(listaletras, ArrayTexto, Comparacion);
        }
        public void CrearDiccionario(List<string> lista, char[] Array, Delegate Comparacion)
        {
            if (Array.Length!=1)
            {
                if (RecorrerTexto(lista, textonuevo, Comparacion))//se tiene que agregar otro caracter
                {
                    if (Array[0] == Convert.ToChar("\0"))
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            Array = Array.Where((source, index) => index != i).ToArray();
                        }
                    }
                    LetrasAgregar(Array);
                    CrearDiccionario(lista, Array, Comparacion);
                }
                else
                {
                    codificacion += NumDiccionario(lista, Comparacion);
                    lista.Add(textonuevo);
                    for (int i = 0; i < numciclo; i++)
                    {
                        Array[i] = default;
                    }
                    char[] aux = new char[Array.Length - numciclo];
                    for (int i = 0; i < numciclo; i++)
                    {
                        Array = Array.Where((source, index) => index != i).ToArray();
                    }
                    textonuevo = Array[0].ToString();
                    numciclo = 0;
                    CrearDiccionario(lista, Array, Comparacion);
                }
            }
            else
            {
                for (int i = 0; i < lista.Count; i++)
                {
                    if (lista[i]==textonuevo)
                    {
                        codificacion += i + 1;
                    }
                }
            }
        }
        public string NumDiccionario(List<string> lista, Delegate Comparacion)
        {
            string numeros = "";
            char[] Aux = textonuevo.ToArray();
            for (int i = 0; i < numciclo; i++)
            {
                for (int j = 0; j < lista.Count ; j++)
                {
                    int comp = Convert.ToInt32(Comparacion.DynamicInvoke(lista[j], Aux[i].ToString()));
                    if (comp == 0)
                    {
                        numeros += (j + 1) + ",";
                    }
                }
            }
            return numeros;
        }
        public bool RecorrerTexto(List<string> lista, string texto, Delegate Comparacion)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                int comp = Convert.ToInt32(Comparacion.DynamicInvoke(lista[i], texto));
                if (comp == 0)//se encontro en el diccionario
                {
                    numciclo++;
                    return true;
                }
            }
            return false;
        }
        public void LetrasAgregar(char[] Array)
        {
            if (Array.Length!=1)
            {
                textonuevo += Array[numciclo];
            }
        }

        public string listadonum() 
        {
            return codificacion;
            
        }
    }
}
