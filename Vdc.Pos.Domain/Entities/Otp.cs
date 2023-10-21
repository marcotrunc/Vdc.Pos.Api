using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities.Common;

namespace Vdc.Pos.Domain.Entities
{
    public class Otp 
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public byte[] OtpCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpiredOn { get; set; }
        public User User { get; set; }
    }
}
