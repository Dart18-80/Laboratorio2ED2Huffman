﻿using System;
using System.Linq;
using ArbolesDeHuffman;
using Laboratorio2ED2.Models;

namespace ConsolaHuffman
{
    class Program
    {
        delegate int DelegadosN(Letras Numero1, Letras Numero2);
        delegate Letras DelegadoLetras(Letras Aux1, Letras Aux2);
        delegate Letras DelegadoBinario(Letras Asignacion, string Codigo);

        static void Main(string[] args)
        {
            try
            { 
                ColaDePrioridad<Letras> ArbolHuff = new ColaDePrioridad<Letras>();
                NodoCP<Letras> nodo = new NodoCP<Letras>();
                Letras AuxiliarDelegados = new Letras();


                Comparable CallNum = new Comparable();

                DelegadoBinario CodigosBinario = new DelegadoBinario(AuxiliarDelegados.AsignarBinario);
                DelegadosN InvocarNumero = new DelegadosN(AuxiliarDelegados.CompareToIndices);//llamado del delegado
                DelegadoLetras SumaProcentrajes = new DelegadoLetras(AuxiliarDelegados.SumaDeIndices);

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
                        decimal numerodecima = decimal.Round(Convert.ToDecimal((CadenaText[i, 0] / totalnumero)), 3);
                        CadenaText[i, 1] = Convert.ToDouble(numerodecima);
                    }
                    for (int i = 0; i < Aux.Length; i++) 
                    {
                        Letras Nuevo = new Letras();
                        Nuevo.index = CadenaText[i, 1];
                        Nuevo.Letra = Convert.ToString(Aux[i]);
                        Nuevo.Binario = "";
                        ArbolHuff.push(ArbolHuff.NodoCPPadre, ArbolHuff.newNode(Nuevo));
                    }

                    ArbolHuff.Heap(ArbolHuff.NodoCPPadre, InvocarNumero);

                    ArbolHuff.ConstruirArbol(ArbolHuff.NodoCPPadre, InvocarNumero, SumaProcentrajes);
                    ArbolHuff.printCode(ArbolHuff.Ultima,"",CodigosBinario);

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
            catch 
            {
                
            }
        }
    }
}
