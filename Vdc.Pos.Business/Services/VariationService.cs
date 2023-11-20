using AutoMapper;
using FluentValidation;
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

namespace Vdc.Pos.Business.Services
{
    public class VariationService : IVariationService
    {
        private readonly IVariationRepository _variationRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<VariationRequestDto> _variationRequestValidator;
        private readonly IValidator<MultiVariationRequestDto> _multiVariationRequestValidator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VariationService(
            IVariationRepository variationRepository,
            ICategoryRepository categoryRepository,
            IValidator<VariationRequestDto> variationRequestValidator,
            IValidator<MultiVariationRequestDto> multiVariationRequestValidator,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _variationRepository = variationRepository;
            _categoryRepository = categoryRepository;
            _variationRequestValidator = variationRequestValidator;
            _multiVariationRequestValidator = multiVariationRequestValidator;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region public methods
        public async Task<IEnumerable<VariationResponseDto>> GetAllVariationsAsync()
        {
            var variations = await _variationRepository.GetAllVariationsAsync();

            if (variations == null || variations.Any() == false)
            {
                throw new Exception("Nessuna variazione presente nel Database");
            }

            return _mapper.Map<VariationResponseDto[]>(variations);
        }

        public async Task<VariationResponseDto> GetVariationByIdAsync(int id)
        {
            var variation = await _variationRepository.GetByPrimaryKeyAsync(id);

            if (variation == null)
            {
                throw new Exception($"Nessuna variazione associata all'id: {id}");
            }
            return _mapper.Map<VariationResponseDto>(variation);
        }

        public async Task<IEnumerable<VariationResponseDto>> GetVaritionsByParentCategoryIdAsync(Guid parentCategoryId)
        {
            if (parentCategoryId == Guid.Empty)
            {
                throw new ArgumentNullException("Parent Category Id inserito risulta nullo");
            }

            var variations = await _variationRepository.GetVaritionsByParentCategoryIdAsync(parentCategoryId);

            if (variations == null || variations.Any() == false)
            {
                var categoryName = _categoryRepository.GetNameOfCategoryByIdAsync(parentCategoryId);
                throw new ArgumentNullException($"nessuna variazione associata alla categoria: {categoryName}");
            }

            return _mapper.Map<VariationResponseDto[]>(variations);

        }

        public async Task<VariationResponseDto> InsertVariationAsync(VariationRequestDto variationRequestDto)
        {
            if (variationRequestDto == null)
            {
                throw new ArgumentNullException("La variazione inserita è nulla");
            }
            
            bool isParentCategoryExisting = await IsCategoryAssociatedExisting(variationRequestDto.ParentCategoryId);

            if (isParentCategoryExisting == false)
            {
                throw new Exception("La categoria inserita non è valida");
            }

            await _variationRequestValidator.ValidateAndThrowAsync(variationRequestDto);

            var variationToAdd = _mapper.Map<Variation>(variationRequestDto);

            var variationAdded = await _variationRepository.InsertAsync(variationToAdd);

            bool isVariationCreated = await _unitOfWork.CommitAsync() > 0;


            if (variationAdded == null || isVariationCreated == false)
            {
                throw new Exception("Variazione non aggiunta");
            }

            return _mapper.Map<VariationResponseDto>(variationAdded.Entity);

        }

        public async Task<IEnumerable<VariationResponseDto>> InsertMultipleVariationAsync(MultiVariationRequestDto multiVarationsRequest)
        {
            if (multiVarationsRequest == null)
            {
                throw new ArgumentNullException("Le variazioni inserite sono nulle");
            }

            bool isParentCategoryExisting = await IsCategoryAssociatedExisting(multiVarationsRequest.ParentCategoryId);

            if (isParentCategoryExisting == false)
            {
                throw new Exception("La variazione inserita non è valida");
            }


            await _multiVariationRequestValidator.ValidateAndThrowAsync(multiVarationsRequest);

            var variationsAdded = new List<Variation>();

            foreach (var varationNameRequest in multiVarationsRequest.Names)
            {
                var variationToAdd = new VariationRequestDto();
                variationToAdd.ParentCategoryId = multiVarationsRequest.ParentCategoryId;
                variationToAdd.Name = varationNameRequest;

                try
                {

                    await _variationRequestValidator.ValidateAndThrowAsync(variationToAdd);
                
                    var variationAdded = await _variationRepository.InsertAsync(_mapper.Map<Variation>(variationToAdd));

                    if (variationAdded != null)
                    {
                        variationsAdded.Add(variationAdded.Entity);
                    }
                }
                catch (Exception ex)
                {
                    //Aggiungere log segnando l'elemento non aggiunto
                    var categoryName = _categoryRepository.GetNameOfCategoryByIdAsync(multiVarationsRequest.ParentCategoryId);
                    Console.WriteLine($"La variazione {varationNameRequest} non è stata aggiunta alla categoria {categoryName} ");
                    continue;
                }

            }

            bool isVariationCreated = await _unitOfWork.CommitAsync() > 0;

            if (isVariationCreated == false)
            {
                throw new Exception("Nessuna variazione è stata aggiunta");
            }

            return _mapper.Map<VariationResponseDto[]>(variationsAdded);

        }

        public async Task<VariationResponseDto> UpdateVariationAsync(int id, VariationRequestDto variationRequestDto)
        {
            if (variationRequestDto == null)
            {
                throw new ArgumentNullException("Parametri inseriti nulli");
            }

            var variation = await _variationRepository.GetByPrimaryKeyAsync(id);

            if (variation == null)
            {
                throw new ArgumentNullException($"Nessuna variazione trovata con l'id: {id}");
            }

            if (variation.ParentCategoryId != variationRequestDto.ParentCategoryId)
            {
                throw new Exception("L'unica modifica effettuabile è il cambio del nome");
            }

            variation.Name = variationRequestDto.Name;

            bool isVariationUpdated = await _unitOfWork.UpdateAsync(variation) > 0;

            if (isVariationUpdated == true)
            {
                return _mapper.Map<VariationResponseDto>(variation);
            }
            else
            {
                throw new Exception($"La Variazione {variation.Name} non è stata modificata");
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

                await _variationRepository.DeleteByPrimaryKeyAsync(id);

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Elemento non cancellato");
            }
        }
        #endregion

        #region private methods
        private async Task<bool> IsCategoryAssociatedExisting(Guid parentCategoryId)
        {
            if (parentCategoryId == Guid.Empty)
            {
                return false;
            }

            var category = await _categoryRepository.GetByPrimaryKeyAsync(parentCategoryId);

            if (category == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
