﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolesDeHuffman
{
    public interface Huffman<T>
    {
        int index { get; set; }
        T SumaDeIndices(T a , T b);
    }
}
