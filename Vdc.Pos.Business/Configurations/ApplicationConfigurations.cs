using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Business.Configurations
{
    public class ApplicationConfigurations
    {
        [Required]
        public string ImagesRoot { get; set; }
    }
}
