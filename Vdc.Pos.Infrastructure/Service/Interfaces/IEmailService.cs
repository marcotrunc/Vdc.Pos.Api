using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vdc.Pos.Infrastructure.Service.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string from, string to, string sub, string body);
    }
}
