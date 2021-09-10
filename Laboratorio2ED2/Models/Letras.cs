﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArbolesDeHuffman;

namespace Laboratorio2ED2.Models
{
    public class Letras : Huffman<Letras>, IComparable
    {
        public double index { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Letra { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int CompareToIndices(int x, int y) 
        {
            int Comparar = x.CompareTo(y);
            if (Comparar == 0)
                return 0;
            else if (Comparar < 0)
                return -1;
            else
                return 1;
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public Letras SumaDeIndices(Letras a, Letras b)
        {
            Letras Nuevo = new Letras();
            Nuevo.index = a.index + b.index;
            Nuevo.Letra = null;
            return Nuevo; 

        }
    }
}