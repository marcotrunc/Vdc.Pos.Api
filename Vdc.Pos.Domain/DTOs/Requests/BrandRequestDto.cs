using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.DTOs.Requests
{
    public class BrandRequestDto
    {
        public string Name{ get; set; }
        public IFormFile? File { get; set; }
    }
}
