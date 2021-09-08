using System;
using ArbolesDeHuffman;
namespace ConsolaHuffman
{
    class Program
    {
        delegate int DelegadosN(int Numero1, int Numero2);

        static void Main(string[] args)
        {
            ColaDePrioridad<int> ArbolHuff = new ColaDePrioridad<int>();
            NodoCP<int> nodo = new NodoCP<int>(); 
            Comparable CallNum = new Comparable();
            DelegadosN InvocarNumero = new DelegadosN(CallNum.CompareToNumero);//llamado del delegado

            Console.WriteLine("Hello World!");
            bool verificacion = true;
            while (verificacion)
            {
                Console.WriteLine("Ingrese un numero");
                int num = Convert.ToInt32(Console.ReadLine());
                

                Console.WriteLine("salida??");
                int salida = Convert.ToInt32(Console.ReadLine());
                if (salida==2)
                {
                    verificacion = false;
                }
            }
        }
    }
}
