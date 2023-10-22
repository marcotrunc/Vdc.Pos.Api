using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Persistence.IRepositories;
using Vdc.Pos.Persistence.Repositories;

namespace Vdc.Pos.Business.Validators
{
    public class UpdatePasswordModuleRequestValidator : AbstractValidator<UpdatePasswordModuleRequestDTO>
    {
        public UpdatePasswordModuleRequestValidator(IUserRepository userRepository)
        {
            RuleFor(u => u.Email)
                .NotNull().WithMessage("La mail non può essere nulla")
                .NotEmpty().WithMessage("La mail non può essere vuota")
                .EmailAddress().WithMessage("Il formato della mail non è corretto")
                .MustAsync(async (email, _) => await userRepository.IsUniqueEmail(email) == false).WithMessage("Nessun Utente registrato con la mail inserita");

            RuleFor(u => u.Password).NotEmpty().WithMessage("La password non può essere vuota")
                   .MinimumLength(8).WithMessage("La password deve avere una lunghezza minima di 8 caratteri")
                   .MaximumLength(16).WithMessage("La password non deve essere più lunga di 16 caratteri")
                   .Matches(@"[A-Z]+").WithMessage("La password deve contenere almeno un carattere maiuscolo")
                   .Matches(@"[a-z]+").WithMessage("La password deve contenere almeno un carattere minuscolo")
                   .Matches(@"[0-9]+").WithMessage("La password deve contenere almeno un numero")
                   .Matches(@"[\!\?\*\.]+").WithMessage("La password deve contenere almeno un carattere speciale (!? *.).");

            RuleFor(model => model.ConfirmPassword)
                   .Equal(model => model.Password)
                   .WithMessage("La conferma della password deve corrispondere alla password.");
        }
    }



}
