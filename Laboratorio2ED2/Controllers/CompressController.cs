using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Laboratorio2ED2.Models;
using ArbolesDeHuffman;
using Laboratorio2ED2.Helpers;

namespace Laboratorio2ED2.Controllers
{
    [ApiController]
    public class CompressController : ControllerBase
    {
        public readonly IHostingEnvironment fistenviroment;
        public CompressController(IHostingEnvironment enviroment)
        {
            this.fistenviroment = enviroment;
        }
        public class FileUlploadAPI
        {
            public IFormFile files { get; set; }
        }
        //Se nombran los delegados que se utilizaran en la compresion
        delegate int DelegadosN(Letras Numero1, Letras Numero2);
        delegate int DelegadoClaseNumero(Letras Numero1);
        delegate Letras DelegadoLetras(Letras Aux1, Letras Aux2);
        delegate Letras DelegadoBinario(Letras Asignacion, string Codigo);
        delegate int DelegadoImpresion(Letras Asignacion, char Codigo);

        [Route("api/compress/{Name}")]
        [HttpPost]
        public IActionResult Post([FromForm] IFormFile files, string name) //para poder llamarlo se necesita poner el noombre files en la key
        {
            ColaDePrioridad<Letras> ArbolHuff = new ColaDePrioridad<Letras>();
            Letras AuxiliarDelegados = new Letras();
            DelegadoClaseNumero Finalizacion = new DelegadoClaseNumero(AuxiliarDelegados.CompareToSalida);
            DelegadoBinario CodigosBinario = new DelegadoBinario(AuxiliarDelegados.AsignarBinario);
            DelegadosN InvocarNumero = new DelegadosN(AuxiliarDelegados.CompareToIndices);//llamado del delegado
            DelegadoLetras SumaProcentrajes = new DelegadoLetras(AuxiliarDelegados.SumaDeIndices);
            DelegadoImpresion Impresion = new DelegadoImpresion(AuxiliarDelegados.CompareToimpresion);

            string uploadsFolder = null;
            string ccc = null;
            if (files != null)
            {
                uploadsFolder = Path.Combine(fistenviroment.ContentRootPath, "Upload");
                string filepath = Path.Combine(uploadsFolder, files.FileName);
                if (!System.IO.File.Exists(filepath))
                {
                    using (var INeadLearn = new FileStream(filepath, FileMode.CreateNew))
                    {
                        files.CopyTo(INeadLearn);
                    }
                }
                ccc = System.IO.File.ReadAllText(filepath);// es el texto del archivo de texto
            }
            //Procedimiento para poder separar el texto recibido
            char[] ArrayCaracteres = ccc.ToCharArray();
            char[] Aux = ArrayCaracteres.Distinct().ToArray();
            int arrayleng = ArrayCaracteres.Length;
            double[,] CadenaText = new double[Aux.Length, 2];
            for (int i = 0; i < Aux.Length; i++)
            {
                int cont = 0;
                for (int j = 0; j < arrayleng; j++)
                {
                    if (Aux[i]==ArrayCaracteres[j])
                    {
                        cont++;
                    }
                }
                CadenaText[i, 0] = cont;
                decimal numerodecimal = decimal.Round(Convert.ToDecimal((CadenaText[i, 0] / arrayleng)), 3);
                CadenaText[i, 1] = Convert.ToDouble(numerodecimal);
            }
            for (int i = 0; i < Aux.Length; i++)
            {
                Letras Nuevo = new Letras();
                Nuevo.index = CadenaText[i, 1];
                Nuevo.Letra = Convert.ToString(Aux[i]);
                Nuevo.Binario = "";
                ArbolHuff.push(ArbolHuff.NodoCPPadre, ArbolHuff.newNode(Nuevo));
            }
            ArbolHuff.Heap(ArbolHuff.NodoCPPadre, InvocarNumero);
            ArbolHuff.ConstruirArbol(ArbolHuff.NodoCPPadre, InvocarNumero, SumaProcentrajes, Finalizacion);
            ArbolHuff.printCode(ArbolHuff.NodoCPPadre, "", CodigosBinario);
            string Codificacion = "";
            for (int i = 0; i < arrayleng; i++)
            {
                ArbolHuff.printCodeLate(ArbolHuff.NodoCPPadre, ArrayCaracteres[i], Impresion);
                Codificacion += ArbolHuff.Enviar.Binario;
            }

            //Se realiza un archivo .huff
            string uploadsNewFolder = Path.Combine(fistenviroment.ContentRootPath, "UploadHuff");

            string direccionNuevo = Path.Combine(uploadsNewFolder, name + ".huff");

            using (StreamWriter outFile = new StreamWriter(direccionNuevo))
                outFile.WriteLine(Codificacion);

            double razonOriginal = (ccc.Length / 8);
            double razonComprimida = (Codificacion.Length) / 8;
            decimal Razcompresion = (Convert.ToDecimal(razonOriginal) / Convert.ToDecimal(razonComprimida));
            decimal Factcompresion = (Convert.ToDecimal(razonComprimida) / Convert.ToDecimal(razonOriginal));
            decimal PorcentajeDism = decimal.Round((Convert.ToDecimal(razonComprimida) / Convert.ToDecimal(razonOriginal) * 100), 2);
            var NuevoArchivo = new CompresionesT
            {
                NombreArchivoOriginal = files.FileName,
                NombreNuevoArchivo = (name + ".huff"),
                RutaArchivoCompremido = direccionNuevo,
                Razondecompresion = Convert.ToInt32(Razcompresion),
                FactordeCompresion=Convert.ToInt32(Factcompresion),
                PorcentajedeRecduccion=Convert.ToInt32(PorcentajeDism)

            };
            Singleton.Intance.DatosCompresiones.Add(NuevoArchivo);

            return Ok("El archivo Huff esta guardado dentro de la carpeta UploadHuff dentro de los archivos del proyecto");
        }
        [Route("api/compressions")]
        [HttpGet]
        public IEnumerable<CompresionesT> GetDatosCompresion() 
        {

         return Singleton.Intance.DatosCompresiones;
        }
    }
}
