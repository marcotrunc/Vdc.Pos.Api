using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Enums;

namespace Vdc.Pos.Domain.DTOs.Requests
{
    public class UserRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = null;
        public RoleEnums Role { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}
