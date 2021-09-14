using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laboratorio2ED2.Models
{
    public class CompresionesT
    {
        public string NombreArchivoOriginal { get; set; }
        public string NombreNuevoArchivo { get; set; }
        public string RutaArchivoCompremido { get; set; }
        public int Razondecompresion { get; set; }
        public int FactordeCompresion { get; set; }
        public int PorcentajedeRecduccion { get; set; }
    }
}
