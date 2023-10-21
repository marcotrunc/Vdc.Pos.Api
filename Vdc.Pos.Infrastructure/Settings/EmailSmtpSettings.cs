using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Infrastructure.Settings
{
    public class EmailSmtpSettings
    {
        public string Smtp  { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int Port { get; set; }
    }
}
