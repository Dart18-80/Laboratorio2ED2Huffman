using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laboratorio2ED2.Helpers
{
    public class Singleton
    {
        private static Singleton _intance;
        public static Singleton Intance
        {
            get
            {
                if (_intance == null) _intance = new Singleton();
                return _intance;
            }
        }

        //Tenes que insertar aqui lo que queres guardar 



    }
}
