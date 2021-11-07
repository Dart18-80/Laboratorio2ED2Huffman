using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Laboratorio2ED2.Models;

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

        public List<CompresionesT> DatosCompresiones = new List<CompresionesT>();
        public List<PorpiedadesCompresion> DatosCompresionesLZW = new List<PorpiedadesCompresion>();


    }
}
