using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.Enums;

namespace Vdc.Pos.Domain.DTOs.Response
{
    public class UserAuthResponseDto
    {
        public Guid Id { get; set; }  
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName {  get; set; } = string.Empty;   
        public RoleEnums Role { get; set; }
        public string? AuthToken { get; set; } = string.Empty;
        public string? RefreshToken { get; set; } = string.Empty;    
    }
}
