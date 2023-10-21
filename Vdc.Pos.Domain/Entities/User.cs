using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Entities.Common;
using Vdc.Pos.Domain.Enums;

namespace Vdc.Pos.Domain.Entities
{
    public class User : EntityBase<Guid>
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsEmailVerified { get; set; } = false;
        public string? PhoneNumber { get; set; } = null;
        public RoleEnums Role { get; set; }
        public DateTime? BirthDate { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public byte[]? PasswordHash { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActived { get; set; } = true;
        public ICollection<Otp> Otps { get; set; }

    }
}
