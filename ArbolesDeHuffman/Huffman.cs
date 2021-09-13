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
        int CompareToIndices(T x, T y);
        int CompareToSalida(T x);
        T AsignarBinario(T b, string a);
    }
}
