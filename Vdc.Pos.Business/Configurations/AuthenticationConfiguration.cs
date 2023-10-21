using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Business.Configurations
{
    public class AuthenticationConfiguration
    {
        [Required]
        public string ClientSecret { get; set; }
    }
}
