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
    public class BrandRequestValidator : AbstractValidator<BrandRequestDto>
    {
        public BrandRequestValidator(IBrandrepository brandRepository)
        {
            RuleFor(b => b.Name)
                .NotEmpty().WithMessage("Il nome della categoria non può essere vuoto")
                .MaximumLength(80).WithMessage("Il nome della categoria non può essere più longo di 80 caratteri")
                .MinimumLength(3).WithMessage("Il nome della categoria deve avere almeno tre caratteri")
                .NotNull().WithMessage($"Il nome della categoria non può essere nullo");
        }
    }
}
