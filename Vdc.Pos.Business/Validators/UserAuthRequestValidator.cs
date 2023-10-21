using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Persistence.IRepositories;

namespace Vdc.Pos.Business.Validators
{
    public class UserAuthRequestValidator : AbstractValidator<UserAuthRequestDto>
    {
        public UserAuthRequestValidator(IUserRepository userRepository)
        {
           
        }
    }
}
