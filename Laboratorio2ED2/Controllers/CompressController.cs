using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Laboratorio2ED2.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPost]

        public IActionResult Post([FromForm] IFormFile files) //para poder llamarlo se necesita poner el noombre files en la key
        {
            string uploadsFolder = null;
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
                string ccc = System.IO.File.ReadAllText(filepath);// es el texto del archivo de texto
            }

            string jaja = "10101010010010010010101010101010010100101010010101";
            string direccionNuevo = Path.Combine(uploadsFolder, files.Name+".huff");
            //Se realiza un archivo .huff
            using (StreamWriter outFile = new StreamWriter(direccionNuevo))
                outFile.WriteLine(jaja);
                return Ok();
        }
    }
}
