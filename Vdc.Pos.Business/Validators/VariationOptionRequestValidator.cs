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
    public class VariationOptionRequestValidator : AbstractValidator<VariationOptionRequestDto>
    {
        public VariationOptionRequestValidator(IVariationOptionsRepository variationOptionsRepository)
        {
            RuleFor(v => v.Value)
                .NotEmpty().WithMessage("Il valore non può essere vuoto")
                .MaximumLength(40).WithMessage("Il valore non può essere più lungo di 80 caratteri")
                .NotNull().WithMessage("Il campo  valore non può essere nullo")
                .MustAsync(async (variationRequestDto, name, _) => await variationOptionsRepository.IsUniqueValueForVariationAsync(variationRequestDto.Value, variationRequestDto.VariationId)).WithMessage("Questa variazione è già esistente per questa categoria");
        }
    }
}
