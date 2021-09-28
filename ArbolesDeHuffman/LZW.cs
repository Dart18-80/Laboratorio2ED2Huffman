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
        public string completo = "";
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
            completo=CodificacionDeTexto(Diccionario);
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
            return completo;  
        }

        public string CodificacionDeTexto(char[] DiccionarioInicial) 
        {
            string TextoCompleto="";//todo el mensaje que se va a mandar en txt
            string TextoParaDecod = "";//caracteristicas para decodificar
            string[] cadenasplit = codificacion.Split(',');
            var max = cadenasplit.Max();
            Array.Sort(DiccionarioInicial);
            string maximoBinario = DecimalBinario(Convert.ToInt32(max));
            string frecuCiacodificada = "";//El codigo en binario
            string diccionario = "";
            for (int i = 0; i < DiccionarioInicial.Length; i++)
            {
                diccionario += DiccionarioInicial[i];
            }
            for (int i = 0; i < cadenasplit.Length; i++)
            {
                string binario = DecimalBinario(Convert.ToInt32(cadenasplit[i]));
                if (binario.Length!=maximoBinario.Length)
                {
                    string aux = CompletarmaxBinario(binario, maximoBinario.Length);
                    binario = aux;
                }
                frecuCiacodificada += binario;
            }
            if (ParInpar(frecuCiacodificada.Length))//Entra si es impar, asi se tiene que completar el byte
            {
                string aux=CompletarTextoBinario(frecuCiacodificada);
                frecuCiacodificada = aux;
            }

            TextoCompleto =CompletarBinario(DecimalBinario(maximoBinario.Length), 1)+CompletarBinario(DecimalBinario(DiccionarioInicial.Length),1) +diccionario+frecuCiacodificada;
            return TextoCompleto;
        }
        public bool ParInpar(int num) 
        {
            if ((num % 2) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public string CompletarTextoBinario(string binario) //completa el codigo completo a un numero par
        {
            char[] cadena = binario.ToCharArray();
            string auxiliar = "";
            string extra = "";
            int delimitador = (binario.Length / 8) + 1;
            for (int i = 0; i < delimitador; i++)
            {
                for (int j = 0; j < cadena.Length; j++)
                {
                    if (i!=(delimitador-1))
                    {
                        auxiliar += Convert.ToString(cadena[j]);
                    }
                    else
                    {
                        extra += Convert.ToString(cadena[j]);
                    }
                }
            }
            string total = auxiliar + CompletarBinario(extra, 0);
            return total;
        }
        public string CompletarBinario(string Binincompleto, int verificar) //llena el sobrante del un byte
        {
            int num = Binincompleto.Length;
            string Completo = "";
            if (num<8)
            {
                int faltante = 8 - num;
                string ceros = "";
                for (int i = 0; i < faltante; i++)
                {
                    ceros += 0;
                }
                if (verificar==0)//los ceros van al final del codigo
                {
                    Completo = Binincompleto + ceros;
                }
                else//los ceros van antes del codigo
                {
                    Completo = ceros +Binincompleto ;
                }
                return Completo;
            }
            return Binincompleto;
        }
        public string CompletarmaxBinario(string Binincompleto, int max) //llena el sobrante del un byte
        {
            int num = Binincompleto.Length;
            string Completo = "";
            int faltante = 8 - num;
            string ceros = "";

            for (int i = 0; i < max-num; i++)
            {
                ceros += 0;
            }
            Completo = ceros + Binincompleto;
            return Completo;
        }
        public static string DecimalBinario(int numero)
        {
            string binario = "";
            if (numero > 0)
            {
                while (numero > 0)
                {
                    if (numero % 2 == 0)
                    {
                        binario = "0" + binario;
                    }
                    else
                    {
                        binario = "1" + binario;
                    }
                    numero = (int)(numero / 2);
                }
                return binario;
            }
            else
            {
                if (numero == 0)
                {
                    return "0";
                }
                else
                {
                    return "";
                }
            }

        }
    }
}
