using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace filepdf.Models.ViewModels
{
    public class EngineerVM
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
