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
        delegate int DelegadosLZW(string Numero1, string Numero2);
        delegate int DelegadoClaseNumero(Letras Numero1);
        delegate Letras DelegadoLetras(Letras Aux1, Letras Aux2);
        delegate Letras DelegadoBinario(Letras Asignacion, string Codigo);
        delegate int DelegadoImpresion(Letras Asignacion, char Codigo);
        delegate int DelegadoDescompresion(Letras Asignacion, char Codigo);
        public static string a = "";


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
            //se buscan las frecuencias de las letras para mandar en el .huff
            string LetrasFrecuencia = "";
            for (int i = 0; i < Aux.Length; i++)
            {
                string Binario = DecimalBinario(Convert.ToInt32(CadenaText[i, 0]));
                LetrasFrecuencia += Convert.ToString(Aux[i]) + "," + Binario+",";
            }

            //Se realiza un archivo .huff
            string uploadsNewFolder = Path.Combine(fistenviroment.ContentRootPath, "UploadHuff");

            string direccionNuevo = Path.Combine(uploadsNewFolder, name + ".huff");

            using (StreamWriter outFile = new StreamWriter(direccionNuevo))
                outFile.WriteLine(LetrasFrecuencia+Codificacion);
            //operaciones para los componentes de los archivos de compresion
            double razonOriginal = (ccc.Length / 8);
            double razonComprimida = (Codificacion.Length) / 8;
            decimal Razcompresion = 0;
            decimal Factcompresion = 0;
            decimal PorcentajeDism = 0;
            if (razonComprimida!=0 && razonOriginal!=0)
            {
                Razcompresion = decimal.Round((Convert.ToDecimal(razonOriginal) / Convert.ToDecimal(razonComprimida)), 3);
                Factcompresion = decimal.Round((Convert.ToDecimal(razonComprimida) / Convert.ToDecimal(razonOriginal)), 3);
                PorcentajeDism = decimal.Round((Convert.ToDecimal(razonComprimida) / Convert.ToDecimal(razonOriginal) * 100), 2);
            }


            //se crea un archivo donde se guaardaran los datos de todos
            string uploadcompresion = Path.Combine(fistenviroment.ContentRootPath, "DatosCompresion");
            string filepathcompresion = Path.Combine(uploadcompresion, "DatosDeLasCompresiones.txt");
            if (System.IO.File.Exists(filepathcompresion))
            {
                System.IO.File.AppendAllText(filepathcompresion, files.FileName + "," + (name + ".huff") + "," + direccionNuevo.ToString() + "," + Razcompresion.ToString() + "," + Factcompresion.ToString() + "," + PorcentajeDism.ToString()+"@");
            }

            return Ok("El archivo Huff esta guardado dentro de la carpeta UploadHuff dentro de los archivos del proyecto");
        }
        [Route("api/compressions")]
        [HttpGet]
        public IEnumerable<CompresionesT> GetDatosCompresion() 
        {
            string uploadcompresion = Path.Combine(fistenviroment.ContentRootPath, "DatosCompresion");
            string filepathcompresion = Path.Combine(uploadcompresion, "DatosDeLasCompresiones.txt");
            if (System.IO.File.Exists(filepathcompresion))
            {
                string ccc = System.IO.File.ReadAllText(filepathcompresion);// es el texto del archivo de texto
                string[] cadenasplit = ccc.Split('@');
                for (int i = 0; i < cadenasplit.Length-1; i++)
                {
                    string[] cadenaAuxiliar = cadenasplit[i].Split(',');

                    var Nuevo = new CompresionesT { 
                    NombreArchivoOriginal=cadenaAuxiliar[0],
                    NombreNuevoArchivo=cadenaAuxiliar[1],
                    RutaArchivoCompremido=cadenaAuxiliar[2],
                    Razondecompresion=Convert.ToDouble(cadenaAuxiliar[3]),
                    FactordeCompresion= Convert.ToDouble(cadenaAuxiliar[4]),
                    PorcentajedeRecduccion=Convert.ToDouble(cadenaAuxiliar[5])
                    };
                    Singleton.Intance.DatosCompresiones.Add(Nuevo);
                }
            }

            return Singleton.Intance.DatosCompresiones;
        }

        [Route("api/descompress")]
        [HttpPost]
        public IActionResult Descomprimirtexto([FromForm] IFormFile files)
        {
            Letras AuxiliarDelegados = new Letras();
            DelegadoDescompresion Descompresion = new DelegadoDescompresion(AuxiliarDelegados.Descompresion);
            ColaDePrioridad<Letras> ArbolHuff = new ColaDePrioridad<Letras>();
            ColaDePrioridad<Letras> ArbolHuffAuxiliar = new ColaDePrioridad<Letras>();
            DelegadosN InvocarNumero = new DelegadosN(AuxiliarDelegados.CompareToIndices);//llamado del delegado
            DelegadoLetras SumaProcentrajes = new DelegadoLetras(AuxiliarDelegados.SumaDeIndices);
            DelegadoClaseNumero Finalizacion = new DelegadoClaseNumero(AuxiliarDelegados.CompareToSalida);
            a = null;
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
                string nuevocccc = ccc.Replace("\r\n","");
                string[] cadenasplit = nuevocccc.Split(',');
                int ultimocadena = cadenasplit.Length-1;
                char[] CodigoBinario =(cadenasplit[ultimocadena]).ToCharArray();//separo el codigo binario a un nuevo char
                double[,] CadenaText = new double[(ultimocadena)/2, 2];//este doule se guarda en la posicion [0,x] la fecuencia y en la posicion [x,1] la probabilidad
                char[] Aux = new char[ultimocadena/2];// en este char se guardan la letras 
                int totalletras = 0;
                int cont = 0, contador=0;

                for (int i = 0; i < ultimocadena; i++)
                {
                    if ((i%2)==0)
                    {
                        Aux[contador] = Convert.ToChar(cadenasplit[i]);
                        contador++;
                    }
                    else
                    {
                        double bindeci = Convert.ToDouble(binariodecimal(cadenasplit[i].ToCharArray()));
                        totalletras += Convert.ToInt32(bindeci);
                        CadenaText[cont, 0] = Convert.ToDouble(bindeci);
                        cont++;
                    }
                }
                for (int i = 0; i < ultimocadena/2; i++)
                {
                    decimal numerodecima = decimal.Round(Convert.ToDecimal((CadenaText[i, 0] / totalletras)), 3);
                    CadenaText[i, 1] = Convert.ToDouble(numerodecima);
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
                ArbolHuffAuxiliar = ArbolHuff;

                BuscarLetras(ArbolHuffAuxiliar.NodoCPPadre, CodigoBinario, ArbolHuff.NodoCPPadre);
                string textodescodificado = a;

                string uploadsNewFolder = Path.Combine(fistenviroment.ContentRootPath, "UploadHuff");
                string[] cadenafile = files.FileName.Split('.');

                string direccionNuevo = Path.Combine(uploadsNewFolder, cadenafile[0].ToString() + ".txt");

                using (StreamWriter outFile = new StreamWriter(direccionNuevo))
                    outFile.WriteLine(textodescodificado);
                return Ok("EL ARCHIVO TXT NUEVO SE GUARDO EN LA CARPETA UPLOADHUFF DEL LABORATORIO");
            }
            else
            {
                return StatusCode(500);

            }
        }

        [Route("api/compress/lzw/{Name}")]
        [HttpPost]
        public IActionResult ComprimirLZW([FromRoute] string name, [FromForm] IFormFile file)
        {
            using var fileRead = file.OpenReadStream();
            try
            {
                LZW testing = new LZW();
                using var reader = new BinaryReader(fileRead);
                var buffer = new byte[2000];
                int existe = 0;
                string encodificador = "";
                List<int> Intermedio = new List<int>();
                while (fileRead.Position < fileRead.Length)
                {
                    buffer = reader.ReadBytes(2000);
                    foreach (var value in buffer)
                    {
                        string trabajo = encodificador + (char)value;
                        existe = testing.Encode(trabajo, encodificador);
                        if (existe != -1)
                        {
                            Intermedio.Add(existe);
                            encodificador = "" + (char)value;
                        }
                        else
                        {
                            encodificador = trabajo;
                        }
                    }
                }
                //INTERMEDIO A BYTES
                List<byte> Aescribir = new List<byte>();

                foreach (int item in Intermedio)
                {
                    Aescribir.AddRange(BitConverter.GetBytes(item));
                }

                //ESCRIBIR COMPRIMIDO

                using var fileWrite = new FileStream(name + ".lzw", FileMode.OpenOrCreate);
                var writer = new BinaryWriter(fileWrite);

                writer.Write(Aescribir.ToArray());

                PorpiedadesCompresion obtener = new PorpiedadesCompresion();
                obtener.Razóndecompresión = (Convert.ToDouble(fileWrite.Length) / Convert.ToDouble(fileRead.Length));
                obtener.Factordecompresión = (Convert.ToDouble(fileRead.Length) / Convert.ToDouble(fileWrite.Length));
                obtener.Porcentajedereducción = (Convert.ToDouble(fileWrite.Length) / Convert.ToDouble(fileRead.Length)) * 100;
                obtener.Nombredelarchivooriginal = (file.FileName);
                obtener.Nombreyrutadelarchivocomprimido = (name + ".lzw");
                Singleton.Intance.DatosCompresionesLZW.Add(obtener);
                writer.Close();
                fileWrite.Close();
                reader.Close();
                fileRead.Close();

                var files = System.IO.File.OpenRead(name + ".lzw");
                return new FileStreamResult(files, "application/lzw")
                {
                    FileDownloadName = name + ".lzw"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/compressions/lzw")]
        [HttpGet]
        public ActionResult Compressionslzw()
        {
            try
            {
                var result = Singleton.Intance.DatosCompresionesLZW.Select(x => new PorpiedadesCompresion { Nombredelarchivooriginal = x.Nombredelarchivooriginal, Nombreyrutadelarchivocomprimido = x.Nombreyrutadelarchivocomprimido, Razóndecompresión = x.Razóndecompresión, Factordecompresión = x.Factordecompresión, Porcentajedereducción = x.Porcentajedereducción });
                if (result == null) return NotFound();      
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [Route("api/decompress/lzw")]
        [HttpPost]
        public IActionResult Decompresion([FromForm] IFormFile file)
        {
            try
            {
                LZW acceder = new LZW();
                string input = file.FileName;
                List<byte> decoding = new List<byte>();
                using var fileRead2 = new FileStream(input, FileMode.OpenOrCreate);
                using var reader2 = new BinaryReader(fileRead2);
                var buffer = new byte[4];
                bool first = true;
                String total = null;
                string output = "";
                int i = 0;

                foreach (PorpiedadesCompresion item in Singleton.Intance.DatosCompresionesLZW)
                {
                    if (item.Nombreyrutadelarchivocomprimido == input)
                    {
                        output = item.Nombredelarchivooriginal;
                        break;
                    }
                    i++;
                }

                var archivo = new FileStream(output, FileMode.OpenOrCreate);
                var escritor = new BinaryWriter(archivo);

                while (fileRead2.Position < fileRead2.Length)
                {
                    buffer = reader2.ReadBytes(4);
                    byte[] plzwork = new byte[] { buffer[0], buffer[1], buffer[2], buffer[3] };
                    if (first)
                    {
                        total = acceder.Firstdeco(BitConverter.ToInt32(plzwork));
                        first = false;
                    }
                    else
                    {
                        total = acceder.Decode(BitConverter.ToInt32(plzwork));
                    }
                    foreach (var item in total)
                    {
                        escritor.Write((byte)item);
                    }

                }

                //DECODIFICAR


                reader2.Close();
                fileRead2.Close(); ;
                escritor.Close();
                archivo.Close();
                var files = System.IO.File.OpenRead(output);
                return new FileStreamResult(files, "application/txt")
                {
                    FileDownloadName = output
                };
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }   
        public static void BuscarLetras(NodoCP<Letras> aux, char[] num, NodoCP<Letras> original)
        {
            if (num!=null)
            {
                char[] Auxchar = new char[num.Length - 1];
                char[] Auxcharletra = new char[num.Length ];

                if (num[0].ToString() == "0")
                {
                    if (aux.Izquierda == null)
                    {
                        a += aux.Data.Letra;

                            for (int j = 0; j < Auxcharletra.Length; j++)
                            {
                                Auxcharletra[j] = num[j];
                            }

                        BuscarLetras(original, Auxcharletra, original);
                    }
                    else
                    {
                        if (num.Length == 1)
                        {
                            Auxchar = null;
                        }
                        else
                        {
                            for (int j = 0; j < Auxchar.Length; j++)
                            {
                                Auxchar[j] = num[j + 1];
                            }
                        }
                        BuscarLetras(aux.Izquierda, Auxchar, original);
                    }
                }
                else
                {
                    if (aux.Derecha == null)
                    {
                        a += aux.Data.Letra;
      
                            for (int j = 0; j < Auxcharletra.Length; j++)
                            {
                                Auxcharletra[j] = num[j ];
                            }

                        BuscarLetras(original, Auxcharletra, original);
                    }
                    else
                    {
                        if (num.Length == 1)
                        {
                            Auxchar = null;
                        }
                        else
                        {
                            for (int j = 0; j < Auxchar.Length; j++)
                            {
                                Auxchar[j] = num[j + 1];
                            }
                        }
                        BuscarLetras(aux.Derecha, Auxchar, original);
                    }
                }
            }
        }
        public static string DecimalBinario(int numero)
        {
            string binario = "";
            if (numero > 0)
            {
                while (numero > 0)
                {
                    if (numero % 2 == 0)
                    {
                        binario = "0" + binario;
                    }
                    else
                    {
                        binario = "1" + binario;
                    }
                    numero = (int)(numero / 2);
                }
                return binario;
            }
            else
            {
                if (numero == 0)
                {
                    return "0";
                }
                else
                {
                    return "";
                }
            }

        }

        public static int binariodecimal(char[] numero) 
        {
            int sum = 0;
            Array.Reverse(numero);
            for (int i = 0; i<numero.Length; i++)
            {
                if (numero[i].ToString() == "1")
                {
                  sum += (int)Math.Pow(2, i);

                }
            }
            return sum;
        }

      
    }

}
