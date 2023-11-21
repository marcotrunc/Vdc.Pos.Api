using AutoMapper;
using FluentValidation;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Business.Services.Interfaces;
using Vdc.Pos.Business.UnitOfWork;
using Vdc.Pos.Business.Validators;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.IRepositories;
using Vdc.Pos.Persistence.Repositories;

namespace Vdc.Pos.Business.Services
{
    public class VariationOptionService : IVariationOptionService
    {
        private readonly IVariationOptionsRepository _variationOptionRepository;
        private readonly IVariationRepository _variationRepository;
        private readonly IValidator<VariationOptionRequestDto> _variationOptionsRequestValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VariationOptionService(
            IVariationOptionsRepository variationOptionRepository,
            IVariationRepository variationRepository,
            IValidator<VariationOptionRequestDto> variationOptionsRequestValidator,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _variationOptionRepository = variationOptionRepository;
            _variationRepository = variationRepository;
            _variationOptionsRequestValidator = variationOptionsRequestValidator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region public methods
        public async Task<IEnumerable<VariationOptionResponseDto>> GetAllVariationOptions()
        {
            var options = await _variationOptionRepository.GetAllOptionsAsync();

            if (options == null || options.Any() == false)
            {
                throw new Exception("Nessuna opzione presente nel database");
            }
            return _mapper.Map<VariationOptionResponseDto[]>(options);
        }

        public async Task<IEnumerable<VariationOptionResponseDto>> GetOptionsByVariationId(int variationId)
        {
            if (variationId <= 0)
            {
                throw new Exception("Variation Id non valido");
            }

            var options = await _variationOptionRepository.GetOptionsByVariationIdAsync(variationId);

            if (options == null || options.Any() == false)
            {
                throw new Exception("Nessuna opzione corrispondente all'Id inserito");
            }
            return _mapper.Map<VariationOptionResponseDto[]>(options);
        }

        public async Task<IEnumerable<VariationOptionResponseDto>> InsertMultipleOptions(MultiOptionRequestDto multiOptionRequest)
        {
            var isVariationExist = await _variationRepository.IsVariationExistsById(multiOptionRequest.VariationId);

            if (isVariationExist == false)
            {
                throw new ArgumentNullException("La variazione selezionata non esiste");
            }

            var optionsAdded = new List<VariationOption>();

            foreach (var value in multiOptionRequest.Values)
            {
                var optionToAdd = new VariationOptionRequestDto()
                {
                    VariationId = multiOptionRequest.VariationId,
                    Value = value
                };

                try
                {
                    await _variationOptionsRequestValidator.ValidateAndThrowAsync(optionToAdd);

                    var optionMapped = _mapper.Map<VariationOption>(optionToAdd);
                    var optionAdded = await _variationOptionRepository.InsertAsync(optionMapped);

                    if (optionAdded != null)
                    {
                        optionsAdded.Add(optionAdded.Entity);
                    }
                }
                catch (Exception ex)
                {
                    //Aggiungere i Log
                    var variationName = _variationRepository.GetNameOfVariationByIdAsync(multiOptionRequest.VariationId);
                    Console.WriteLine($"L'opzione {value} non è stata aggiunta alla variazione {variationName}");
                    continue;
                }

            }

            bool isOptionsCreated = await _unitOfWork.CommitAsync() > 0;

            if (isOptionsCreated == false)
            {
                throw new Exception("Nessuna Opzione è stata aggiunta");
            }

            return _mapper.Map<VariationOptionResponseDto[]>(optionsAdded);
        }

        public async Task<VariationOptionResponseDto> UpdateVariationOptionAsync(int id, VariationOptionRequestDto variationOptionRequest)
        {
            if (variationOptionRequest == null)
            {
                throw new ArgumentNullException("Parametri inseriti nulli");
            }

            var option = await _variationOptionRepository.GetByPrimaryKeyAsync(id);

            if (option == null)
            {
                throw new ArgumentNullException($"Nessuna opzione trovata con il seguente id: {id}");
            }

            if (option.VariationId != variationOptionRequest.VariationId)
            {
                throw new ArgumentException("L' unica modifica effettuabile è il cambio valore");
            }

            option.Value = variationOptionRequest.Value;

            bool isOptionUpdated = await _unitOfWork.UpdateAsync(option) > 0;

            if (isOptionUpdated == true)
            {
                return _mapper.Map<VariationOptionResponseDto>(option);
            }
            else
            {
                throw new Exception($"L'opzione {option.Value} non è stata modificata");
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    throw new ArgumentNullException("Id inserito non valido");
                }

                await _variationOptionRepository.DeleteByPrimaryKeyAsync(id);

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Elemento non cancellato");
            }
        }
        #endregion

        #region private methods
        #endregion
    }
}
