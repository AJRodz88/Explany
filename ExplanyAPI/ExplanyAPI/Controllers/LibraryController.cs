using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace ExplanyAPI.Controllers
{
    [ApiController]
    [Route("Library")]
    public class LibraryController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private Dictionary<string, string> Library;
        public LibraryController(ILogger<WeatherForecastController> logger)
        {
            Library = new Dictionary<string, string>();
            _logger = logger;

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "ExplanyAPI.library.txt";
            string result;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }

            string[] single = new string[2];
            var list = result.Split('\n');
            foreach(var l in list)
            {
                single = l.Split(':');
                Library[single[0]] = single[1];
            }
        }

        [HttpGet]
        public string GetLibrary()
        {
            return JsonConvert.SerializeObject(Library);
        }
    }
}
