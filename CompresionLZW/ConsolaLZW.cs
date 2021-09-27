using System;
using System.Collections.Generic;
using System.Text;
using ArbolesDeHuffman;
using Laboratorio2ED2.Models;

namespace CompresionLZW
{
    class ConsolaLZW
    {
        delegate int DelegadosN(string Numero1, string Numero2);

        static void Main(string[] args)
        {
            LZW<Texto> CompresionLZW = new LZW<Texto>();
            Texto AuxiliarDelegados = new Texto();
            DelegadosN InvocarNumero = new DelegadosN(AuxiliarDelegados.CompareToLetras);//llamado del delegado
            Console.WriteLine("Compresion de LZW");


            Console.WriteLine("Ingrese el texto que desea comprimir:");
            string text = Convert.ToString(Console.ReadLine());
            CompresionLZW.CrearLZW(text, InvocarNumero);
            Console.WriteLine(CompresionLZW.listadonum());
            Console.ReadLine();



        }
    }
}
