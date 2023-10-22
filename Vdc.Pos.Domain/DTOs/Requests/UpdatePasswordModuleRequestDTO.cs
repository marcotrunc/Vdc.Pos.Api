using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Domain.DTOs.Requests
{
    public class UpdatePasswordModuleRequestDTO
    {
        public string Email  { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
