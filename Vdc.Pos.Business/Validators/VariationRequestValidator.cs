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
    public class VariationRequestValidator : AbstractValidator<VariationRequestDto>
    {
        public VariationRequestValidator(IVariationRepository variationRepository) 
        {
            RuleFor(v => v.Name)
                .NotEmpty().WithMessage("Il nome non può essere vuoto")
                .MaximumLength(80).WithMessage("Il nome non può essere più lungo di 80 caratteri")
                .MinimumLength(1).WithMessage("Il nome della variazione non può essere più corta di 2 caratteri")
                .NotNull().WithMessage("Il campo  Nome non può essere nullo")
                .MustAsync(async (variationRequestDto, name, _) => await variationRepository.IsUniqueNameForParentCategoryAsync(variationRequestDto.Name, variationRequestDto.ParentCategoryId)).When(v => v.ParentCategoryId != null).WithMessage("Questa variazione è già esistente per questa categoria");
        }
    }
}
