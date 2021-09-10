using System;
using System.Linq;
using ArbolesDeHuffman;
using Laboratorio2ED2.Models;

namespace ConsolaHuffman
{
    class Program
    {
        delegate int DelegadosN(int Numero1, int Numero2);

        static void Main(string[] args)
        {
            ColaDePrioridad<Letras> ArbolHuff = new ColaDePrioridad<Letras>();
            NodoCP<Letras> nodo = new NodoCP<Letras>(); 
            Comparable CallNum = new Comparable();
            DelegadosN InvocarNumero = new DelegadosN(CallNum.CompareToNumero);//llamado del delegado

            Console.WriteLine("Codificacion de Huffman");
            bool verificacion = true;

            while (verificacion)
            {
                Console.WriteLine("Ingrese el texto que desea comprimir:");
                string text = Convert.ToString(Console.ReadLine()).ToUpper();
                char[] arrayCaracteres = text.ToCharArray();
                char[] Aux = arrayCaracteres.Distinct().ToArray();

                int totalnumero = arrayCaracteres.Length;
                int totalarrayNuevo = Aux.Length;
                double[,] CadenaText = new double[totalarrayNuevo, 2];

                for (int i = 0; i < totalarrayNuevo; i++)
                {
                    int cont = 0;
                    for (int j = 0; j < arrayCaracteres.Length; j++)
                    {
                        if (Aux[i] == arrayCaracteres[j])
                        {
                            cont++;
                        }
                    }
                    CadenaText[i, 0] = cont;
                    CadenaText[i, 1] = (CadenaText[i, 0]/ totalnumero);
                }
                //para mandar la letra se llama Aux[i]      String
                //para llamar a la frecuencia CadenaText[i,0]    Double
                //para llamar a la probabilidad Cadenatext[i,1]    Double

                Console.WriteLine("desea salir? presione 1");
                int a = Convert.ToInt32(Console.ReadLine());
                if (a==1)
                {
                    verificacion = false;
                }
            }
        }
    }
}
