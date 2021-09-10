using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolesDeHuffman
{
    public interface Huffman<T>
    {
        double index { get; set; }
        string Binario { get; set; }
        T SumaDeIndices(T a , T b);
    }
}
