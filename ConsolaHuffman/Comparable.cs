using System;
using System.Collections.Generic;
using System.Text;

namespace ConsolaHuffman
{
    class Comparable : IComparable
    {
        public int CompareToNumero(int num1, int num2)
        {
            return num1.CompareTo(num2);
        }
        public int CompareTo(object obj)
        {
            if (Convert.ToInt16(this.CompareTo(obj)) > 0)
                return 1;
            else if (Convert.ToInt16(this.CompareTo(obj)) < 0)
                return -1;
            else
                return 0;
        }
    }
}
