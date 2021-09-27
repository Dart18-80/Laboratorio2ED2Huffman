using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArbolesDeHuffman
{
    public interface ILZW
    {
        string Archivo { get; set; }

        int CompareToLetras(string Letra1, string Letra2);
    }
}
