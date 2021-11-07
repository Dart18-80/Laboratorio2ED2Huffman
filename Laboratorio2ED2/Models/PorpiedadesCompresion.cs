using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Laboratorio2ED2.Models
{
    public class PorpiedadesCompresion
    {
        public string Nombredelarchivooriginal { get; set; }
        public string Nombreyrutadelarchivocomprimido { get; set; }
        public double Razóndecompresión { get; set; }
        public double Factordecompresión { get; set; }
        public double Porcentajedereducción { get; set; }

        public PorpiedadesCompresion(string NmbreOrigianl, string NombreComp, double razon, double factor, double porcentaje)
        {

            Nombredelarchivooriginal = NmbreOrigianl;
            Nombreyrutadelarchivocomprimido = NombreComp;
            Razóndecompresión = razon;
            Factordecompresión = factor;
            Porcentajedereducción = porcentaje;
        }
        public PorpiedadesCompresion() { }
    }
}
