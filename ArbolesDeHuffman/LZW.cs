using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ArbolesDeHuffman
{
    public class LZW <T> where T : IComparable 
    {
        public string textonuevo = "";
        public string completo = "";
        public void CrearLZW(string texto, Delegate Comparacion) 
        {
            bool Verificar = texto.Contains("\r\n");

            List<string> listaletras = new List<string>();
            List<string> listatexto = new List<string>();
            List<string> lisdiccio = new List<string>();

            if (Verificar)
            {
                char[] ArrayTexto = texto.ToCharArray();//El texto completo en un array 
                char[] DiccionarioVeridifcar = ArrayTexto.Distinct().ToArray();

                for (int i = 0; i < DiccionarioVeridifcar.Length-1; i++)
                {
                    Array.Sort(DiccionarioVeridifcar);

                    if (i==0)
                    {
                        listaletras.Add("\r\n");
                        lisdiccio.Add("\r\n");
                    }
                    else
                    {
                        listaletras.Add(DiccionarioVeridifcar[i+1].ToString());
                        lisdiccio.Add(DiccionarioVeridifcar[i + 1].ToString());
                    }
                }

                listaletras.Sort();

                for (int i = 0; i < ArrayTexto.Length-2; i++)
                {
                    if (ArrayTexto[i].ToString()=="\r")
                    {
                        string ex = ArrayTexto[i].ToString() + ArrayTexto[i + 1].ToString();
                        listatexto.Add(ex);
                        i++;
                    }
                    else if (ArrayTexto[i].ToString() == "\n")
                    {
                    }
                    else
                    {
                        listatexto.Add(ArrayTexto[i].ToString());
                    }
                }

                textonuevo = ArrayTexto[0].ToString();
                lisdiccio.Sort();

                CrearDiccionario(listaletras, listatexto, Comparacion);
                completo = CodificacionDeTexto(lisdiccio);
            }
            else
            {
                char[] ArrayTexto = texto.ToCharArray();//El texto completo en un array 
                char[] Diccionario = ArrayTexto.Distinct().ToArray();//El primer diccionario

                for (int i = 0; i < Diccionario.Length; i++)
                {
                    listaletras.Add(Diccionario[i].ToString());
                    lisdiccio.Add(Diccionario[i].ToString());
                }
                for (int i = 0; i < ArrayTexto.Length; i++)
                {
                    listatexto.Add(ArrayTexto[i].ToString());
                }

                listaletras.Sort();
                lisdiccio.Sort();

                textonuevo = ArrayTexto[0].ToString();
                CrearDiccionario(listaletras, listatexto, Comparacion);
                completo = CodificacionDeTexto(lisdiccio) ;
            }
        }
        public string codificacion = "";
        public int numciclo = 0;
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
                bool Verificar = textonuevo.Contains("\r\n");

                if (Verificar)
                {
                    List<string> Aux = new List<string>();
                    char[] Auxilia = textonuevo.ToArray();
                    for (int i = 0; i < Auxilia.Length; i++)
                    {
                        if (Auxilia[i].ToString() == "\r")
                        {
                            Aux.Add("\r\n");
                            i++;
                        }
                        else
                        {
                            Aux.Add(Auxilia[i].ToString());
                        }
                    }
                    for (int i = 0; i < Aux.Count - 1; i++)
                    {
                        listatexto.RemoveAt(0);
                    }
                }
                else
                {
                    for (int i = 0; i < textonuevo.Length - 1; i++)
                    {
                        listatexto.RemoveAt(0);
                    }
                }
                if (listatexto.Count!=0)
                {
                    textonuevo = listatexto[0];
                }
                numciclo = 0;
                CrearDiccionario(lista, listatexto, Comparacion);
            }
        }
        public string NumDiccionario(List<string> lista, Delegate Comparacion)
        {
            string numeros = "";
            string palabrabusc = "";
            bool Verificar = textonuevo.Contains("\r\n");
            int ver = textonuevo.IndexOf("\r\n");
            List<string> Aux = new List<string>();
            char[] Auxilia = textonuevo.ToArray();

            if (Verificar)
            {
                for (int i = 0; i < Auxilia.Length; i++)
                {
                    if (Auxilia[i].ToString()=="\r")
                    {
                        Aux.Add("\r\n");
                        i++;
                    }
                    else
                    {
                        Aux.Add(Auxilia[i].ToString());
                    }
                }
            }
            else
            {
                for (int i = 0; i < Auxilia.Length; i++)
                {
                    Aux.Add(Auxilia[i].ToString());
                }
            }

            for (int i = 0; i < Aux.Count-1; i++)
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

        public string CodificacionDeTexto(List<string> lista) 
        {
            string TextoCompleto="";//todo el mensaje que se va a mandar en txt
            string TextoParaDecod = "";//caracteristicas para decodificar
            string[] cadenasplit = codificacion.Split(',');
            int[] auxi = new int[cadenasplit.Length];

            for (int i = 0; i < cadenasplit.Length; i++)
            {
                auxi[i] = Convert.ToInt32(cadenasplit[i]);
            }
            var max = auxi.Max() ;

            string maximoBinario = DecimalBinario(Convert.ToInt32(max));
            string frecuCiacodificada = "";//El codigo en binario
            string diccionario = "";

            for (int i = 0; i < lista.Count; i++)
            {
                diccionario += lista[i];
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
            int delimitador = (frecuCiacodificada.Length / 8) + 1;
            int totalbits = delimitador * 8;

            if (delimitador!=totalbits)//Entra si es impar, asi se tiene que completar el byte
            {
                string aux=CompletarTextoBinario(frecuCiacodificada);
                frecuCiacodificada = aux;
            }

            TextoCompleto =CompletarBinario(DecimalBinario(maximoBinario.Length), 1)+CompletarBinario(DecimalBinario(lista.Count),1) +diccionario+frecuCiacodificada;
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
            int totalbits = delimitador * 8;
            string total = binario;

            for (int i = 0; i < totalbits-binario.Length; i++)
            {
                total += 0;
            }
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
        public string texto = "";

        public void Descomprimirlzw(string ccc)
        {
            char[] Arraycompleto = ccc.ToCharArray();
            string PrimerByte = default;
            string SegundoByte = default;


            for (int i = 0; i < 8; i++)
            {
                PrimerByte += Arraycompleto[i].ToString();
                SegundoByte += Arraycompleto[8 + i].ToString();
            }

            int CantDiccionario = binariodecimal(SegundoByte);
            int CantBits = binariodecimal(PrimerByte);

            List<string> diccionario = new List<string>();
            string paraverificara = "";
            for (int i = 0; i < Arraycompleto.Length-2; i++)
            {
                paraverificara += Arraycompleto[i].ToString();
            }
            bool Verificar = paraverificara.Contains("\r\n");

            if (Verificar)
            {
                int posicionespacio = ccc.IndexOf("\r\n") ;
                for (int i = 0; i <= CantDiccionario; i++)
                {
                    if ((16+i)==posicionespacio)
                    {
                        string enter = (Arraycompleto[16 + i].ToString() + Arraycompleto[16 + i+1].ToString());
                        diccionario.Add(enter);
                        i++;
                    }
                    else
                    {
                        diccionario.Add(Arraycompleto[16 + i].ToString());

                    }
                }
            }
            else
            {
                for (int i = 0; i < CantDiccionario; i++)
                {
                    diccionario.Add(Arraycompleto[16 + i].ToString());
                }
            }

            int Sobrante = Arraycompleto.Length-(16 + CantDiccionario);
            string CodigoBinario = default;

            bool Versalto = diccionario.Contains("\r\n");

            if (Versalto)
            {
                for (int i = 0; i < Sobrante-3; i++)
                {
                    CodigoBinario += Arraycompleto[(16 + CantDiccionario) + i+1];
                }
            }
            else
            {
                for (int i = 0; i < Sobrante; i++)
                {
                    CodigoBinario += Arraycompleto[(16 + CantDiccionario) + i];
                }
            }
           
            CodigoBinario = Regex.Replace(CodigoBinario, "\r\n", string.Empty);

            List<string> BinarioFragmentado = new List<string>();
            char[] aux = CodigoBinario.ToCharArray();

            string bitnumero = "";
            int cont = 1;

            for (int j = 0; j < aux.Length; j++)
            {
                bitnumero += aux[j].ToString();
                if (cont > (CantBits - 1))
                {
                    BinarioFragmentado.Add(bitnumero);
                    bitnumero = "";
                    cont = 0;
                }
                cont++;
            }

            Almacenamientodiccionario(diccionario, BinarioFragmentado);
        }
        public string Previo ;
        public string Actual = "";
        public string Nuevo= "";
        public int n = 0;
        public void Almacenamientodiccionario(List<string> diccionario, List<string> binarios) 
        {
            if (binarios.Count!=0)
            {
                int val = binariodecimal(binarios[0].ToString());
                if (Previo==null)
                {
                    Actual += BuscarLetra(diccionario, val.ToString());
                    Previo = Actual;
                    texto += Actual;
                    n++;
                    binarios.RemoveAt(0);
                    Almacenamientodiccionario(diccionario, binarios);
                }
                else
                {
                    if (val>diccionario.Count)
                    {
                        Actual = BuscarLetra(diccionario, val.ToString());
                        Nuevo = Previo + Actualunico(Previo);
                        diccionario.Add(Nuevo);
                        texto += Nuevo;
                        Previo = Nuevo;
                        binarios.RemoveAt(0);
                        Almacenamientodiccionario(diccionario, binarios);
                    }
                    else
                    {
                        Actual = BuscarLetra(diccionario, val.ToString());
                        Nuevo = Previo + Actualunico(Actual);
                        diccionario.Add(Nuevo);
                        texto += Actual;
                        Previo = Actual;
                        n++;
                        binarios.RemoveAt(0);
                        Almacenamientodiccionario(diccionario, binarios);
                    }
                   
                }
            }
        }
        public string Actualunico(string actual) 
        {
            char[] ac = actual.ToCharArray();
            int num = ac.Length;
            string a = "";
            if (num!=1)
            {
                for (int i = 0; i < num-1; i++)
                {
                    a+= ac[i];
                }
                return a;
            }
            else
            {
                return actual;
            }

        }
        public string BuscarLetra(List<string> diccionario, string num) 
        {
            string val = "";
            for (int i = 0; i < diccionario.Count; i++)
            {
                if ((Convert.ToInt32(num)-1) == i)
                {
                    val += diccionario[i];
                }
            }
            return val;
        }
        public static int binariodecimal(string bin)
        {
            int sum = 0;
            char[] numero = bin.ToCharArray();
            Array.Reverse(numero);
            for (int i = 0; i < numero.Length; i++)
            {
                if (numero[i].ToString() == "1")
                {
                    sum += (int)Math.Pow(2, i);

                }
            }
            return sum;
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
