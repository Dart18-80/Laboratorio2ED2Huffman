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
            List<string> listaletras = new List<string>();
            List<string> listatexto = new List<string>();

            for (int i = 0; i < Diccionario.Length; i++)
            {
                listaletras.Add(Diccionario[i].ToString());
            }
            for (int i = 0; i < ArrayTexto.Length; i++)
            {
                listatexto.Add(ArrayTexto[i].ToString());
            }
            listaletras.Sort();
            textonuevo = ArrayTexto[0].ToString();
            CrearDiccionario(listaletras, listatexto, Comparacion);
        }
        public void CrearDiccionario(List<string> lista, List<string> listatexto, Delegate Comparacion)
        {
            if (RecorrerTexto(lista, textonuevo, Comparacion))//se tiene que agregar otro caracter
            {
                if (listatexto.Count==textonuevo.Length)
                {
                    for (int i = 0; i < lista.Count; i++)
                    {
                        if (lista[i]==textonuevo)
                        {
                            codificacion += i+1;
                        }
                    }
                }
                else
                {
                    LetrasAgregar(listatexto);
                    CrearDiccionario(lista, listatexto, Comparacion);
                }
                
            }
            else
            {
                codificacion += NumDiccionario(lista, Comparacion);
                lista.Add(textonuevo);
                for (int i = 0; i < textonuevo.Length - 1; i++)
                {
                    listatexto.RemoveAt(0);
                }
                textonuevo = listatexto[0];
                numciclo = 0;
                CrearDiccionario(lista, listatexto, Comparacion);
            }
        }
        public string NumDiccionario(List<string> lista, Delegate Comparacion)
        {
            string numeros = "";
            string palabrabusc = "";
            char[] Aux = textonuevo.ToArray();
            for (int i = 0; i < Aux.Length-1; i++)
            {
                 palabrabusc += Aux[i].ToString();
            }
            for (int j = 0; j < lista.Count; j++)
            {
                int comp = Convert.ToInt32(Comparacion.DynamicInvoke(lista[j], palabrabusc));
                if (comp == 0)
                {
                    numeros += (j + 1) + ",";
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
        public void LetrasAgregar(List<string> lista)
        {
                textonuevo += lista[numciclo];
        }

        public string listadonum() 
        {
            return codificacion;
            
        }

        public string CodificacionDeTexto(char[] DiccionarioInicial) 
        {
            string TextoCodificado="";
            string[] cadenasplit = codificacion.Split(',');
            var max = cadenasplit.Max();
            if (Convert.ToInt32(max)<=255)//Se encuentra dentro de un byte 11111111
            {

            }
            else//El numero se excede de un byte
            {

            }
            return TextoCodificado;
        }

        public void decimalabinario() 
        {
        }
    }
}
