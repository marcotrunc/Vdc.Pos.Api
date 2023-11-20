using Azure.Core;
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
    public class MultiVariationRequestValidator : AbstractValidator<MultiVariationRequestDto>
    {
        public MultiVariationRequestValidator(IVariationRepository variationRepository)
        {
            RuleForEach(v => v.Names)
                .NotEmpty().WithMessage("Il nome non può essere vuoto")
                .MaximumLength(80).WithMessage("Il nome non può essere più lungo di 80 caratteri")
                .MinimumLength(1).WithMessage("Il nome della variazione non può essere più corta di 2 caratteri")
                .NotNull().WithMessage("Il campo  Nome non può essere nullo");

            RuleFor(v => v.ParentCategoryId)
                .NotEmpty().WithMessage("Il campo parent category Id non può essere vuoto")
                .NotNull().WithMessage("Il campo parent category Id non può essere nullo");
        }
    }
}
