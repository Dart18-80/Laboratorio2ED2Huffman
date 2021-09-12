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
    public class CodificacionController : ControllerBase
    {
        public readonly IHostingEnvironment fistenviroment;
        public CodificacionController(IHostingEnvironment enviroment)
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
            if (files!=null)
            {
                string uploadsFolder = Path.Combine(fistenviroment.ContentRootPath, "Upload");
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
            return Ok();
        }
    }
}
