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
    public class UserRequestValidator : AbstractValidator<UserRequestDto>
    {
        public UserRequestValidator(IUserRepository userRepository) 
        {
            RuleFor(u => u.Name)
                .NotEmpty().WithMessage("Il nome non può essere vuoto")
                .MaximumLength(80).WithMessage("Il nome non può essere più longo di 80 caratteri")
                .MinimumLength(1).WithMessage("Il nome deve avere almeno un carattere")
                .NotNull().WithMessage($"Il nome non può essere nullo");

            RuleFor(u => u.LastName)
               .NotEmpty().WithMessage("Il cognome non può essere vuoto")
               .MaximumLength(120).WithMessage("Il cognome non può essere più longo di 120 caratteri")
               .MinimumLength(1).WithMessage("Il cognome deve avere almeno un carattere")
               .NotNull().WithMessage($"Il cognome non può essere nullo");

            RuleFor(u => u.Email)
                .NotNull().WithMessage("La mail non può essere nulla")
                .NotEmpty().WithMessage("La mail non può essere vuota")
                .EmailAddress().WithMessage("Il formato della mail non è corretto")
                .MustAsync(async (email,_) => await userRepository.IsUniqueEmail(email)).WithMessage("Questa mail è già stata inserita");
                    

            RuleFor(u => u.BirthDate).LessThan(DateTime.UtcNow).When(u => u.BirthDate != null).WithMessage("La data di Nascita non può superare la data odierna");

            RuleFor(u => u.PhoneNumber).MaximumLength(20).MinimumLength(10).When(u => !String.IsNullOrEmpty(u.PhoneNumber)).WithMessage("Il numero di cellulare deve avere tra i 10 e 20 Caratteri");
        }
    }
}
