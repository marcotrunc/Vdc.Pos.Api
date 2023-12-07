using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using Slugify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Business.Configurations;
using Vdc.Pos.Business.Services.Interfaces;
using Vdc.Pos.Business.UnitOfWork;
using Vdc.Pos.Business.Utilities;
using Vdc.Pos.Business.Validators;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Infrastructure.Settings;
using Vdc.Pos.Persistence.IRepositories;
using Vdc.Pos.Persistence.Repositories;

namespace Vdc.Pos.Business.Services
{
    public class BrandService : IBrandService
    {
        private readonly ApplicationConfigurations _applicationConfiguration;
        private readonly IBrandrepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<BrandRequestDto> _brandValidator;
        private readonly IMapper _mapper;
        

        public BrandService(
            IOptions<ApplicationConfigurations> applicationConfiguration,
            IBrandrepository brandRepository, 
            IUnitOfWork unitOfWork,
            IValidator<BrandRequestDto> brandValidator,
            IMapper mapper)
        {
            _applicationConfiguration = applicationConfiguration.Value;
            _brandRepository = brandRepository;
            _unitOfWork = unitOfWork;
            _brandValidator = brandValidator;
            _mapper = mapper;
        }

        #region public Methods
        public async Task<IEnumerable<BrandResponseDto>> GetAllBrandsAsync()
        {
            var brands = await _brandRepository.GetAllAsync();

            if (brands == null || brands.Any() == false)
            {
                throw new ArgumentNullException("Nessun Brand presente nel Database");
            }

            return _mapper.Map<BrandResponseDto[]>(brands);
        }

        public async Task<IEnumerable<BrandResponseDto>> GetBrandsNotDeletedAsync()
        {
            var brands = await _brandRepository.GetBrandsNotDeletedAsync();

            if (brands == null || brands.Any() == false)
            {
                throw new ArgumentNullException("Nessun Brand attivo presente nel Database");
            }
            return _mapper.Map<BrandResponseDto[]>(brands);
        }

        public async Task<BrandResponseDto> GetBandByPrimaryAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException($"L'id {id} non è valido");
            }
            var brand = await _brandRepository.GetByPrimaryKeyAsync(id);

            if (brand == null)
            {
                throw new ArgumentNullException($"Nessun brand corrispondente all'id {id}");
            }
            return _mapper.Map<BrandResponseDto>(brand);
        }

        public async Task<BrandResponseDto> InsertBrandAsync(BrandRequestDto brandRequestDto)
        {
            if (brandRequestDto == null)
            {
                throw new ArgumentNullException("La richiesta non puù essere nulla");
            }

            await _brandValidator.ValidateAndThrowAsync(brandRequestDto);

            var isUniqueBrandName = await _brandRepository.IsUniqueBrandName(brandRequestDto.Name);

            if (isUniqueBrandName == false)
            {
                throw new ArgumentNullException("Il nome inserito del Brand è già in uso");
            }

            Brand brandToAdd = new Brand()
            {
                Name = brandRequestDto.Name,
                Slug = brandRequestDto.Name
            };

            var firstStepBrandAdded = await _brandRepository.InsertAsync(brandToAdd);

            if (firstStepBrandAdded == null)
            {
                throw new ArgumentException("Brand non aggiunto");
            }

            bool isBrandAdded = await _unitOfWork.CommitAsync() > 0;

            if (isBrandAdded == true && brandRequestDto.File == null)
            {
                return _mapper.Map<BrandResponseDto>(firstStepBrandAdded.Entity);
            }

            var fileExtenstion = Path.GetExtension(brandRequestDto.File.FileName).Substring(1);

            if (CustomValidator.IsImageExtensionValid(fileExtenstion) == false)
            {
                throw new Exception("Estensione non valida, le uniche estensioni valide sono: jpeg, jpg e png");
            }

            bool isBrandImagesUpdated = Utility.UploadDocument(brandRequestDto.File, Path.Combine(_applicationConfiguration.ImagesRoot, "Brands"), firstStepBrandAdded.Entity.Slug, out string fileNameWithPath);

            bool secondStepBrandAdded = false;

            if (isBrandImagesUpdated == true)
            {
                brandToAdd.ImgPath = fileNameWithPath;
                secondStepBrandAdded = await _unitOfWork.UpdateAsync(brandToAdd) > 0;
            }

            if (secondStepBrandAdded == true)
            {
                return _mapper.Map<BrandResponseDto>(firstStepBrandAdded.Entity);
            }
            else
            {
                throw new Exception("Brand salvata, ma immagine non caricata");
            }

        }
        public async Task<BrandResponseDto> UpdateBrandAsync(Guid id, BrandRequestDto brandToUpdate)
        {
            if(id == Guid.Empty || brandToUpdate == null)
            {
                throw new ArgumentNullException($"{(id == Guid.Empty ? "Id Inserito non valido" : "Parametri inseriti nulli")}");
            }

            var brand = await _brandRepository.GetByPrimaryKeyAsync(id);

            if(brand is null)
            {
                throw new ArgumentNullException($"Nessun brand associata a questo id");
            }

            await _brandValidator.ValidateAndThrowAsync(brandToUpdate);

            brand.Name = brandToUpdate.Name;
            brand.Slug = brandToUpdate.Name;

            var isBrandUpdated = await _unitOfWork.UpdateAsync(brand) > 0;

            if(isBrandUpdated == true && brandToUpdate.File == null)
            {
                return _mapper.Map<BrandResponseDto>(brand);
            }

            var fileExtenstion = Path.GetExtension(brandToUpdate.File.FileName).Substring(1);

            if (CustomValidator.IsImageExtensionValid(fileExtenstion) == false)
            {
                throw new Exception("Estensione non valida, le uniche estensioni valide sono: jpeg, jpg e png");
            }

            bool isBrandImagesUpdated = Utility.UploadDocument(brandToUpdate.File, Path.Combine(_applicationConfiguration.ImagesRoot, "Brands"), brand.Slug, out string fileNameWithPath);
            
            bool secondStepBrandAdded = false;

            if (isBrandImagesUpdated == true)
            {
                brand.ImgPath = fileNameWithPath;
                secondStepBrandAdded = await _unitOfWork.UpdateAsync(brand) > 0;
            }

            if (secondStepBrandAdded == true)
            {
                return _mapper.Map<BrandResponseDto>(brand);
            }
            else
            {
                throw new Exception("Brand modificato, ma immagine non caricata");
            }
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    throw new ArgumentNullException("Id inserito non valido");
                }

                var brand = await _brandRepository.GetByPrimaryKeyAsync(id);
                
                if (brand == null)
                {
                    throw new ArgumentException("Nessun Brand corrispondente all'id inserito");
                }

                bool isBrandUpdated = false;
                string errorMessage = "Cancellazione non andata a buon fine, contattare l'assistenza";


                if (brand.IsDeleted == false)
                {
                    brand.IsDeleted = true;
                    isBrandUpdated = await _unitOfWork.UpdateAsync(brand) > 0;
                    return isBrandUpdated ? true : throw new ArgumentException(errorMessage);
                }

                
                await _brandRepository.DeleteByPrimaryKeyAsync(id);
                isBrandUpdated = await _unitOfWork.CommitAsync() > 0;

                bool isBrandImagesDeleted = false;

                if (isBrandUpdated == true && String.IsNullOrEmpty(brand.ImgPath) == false)
                {
                    isBrandImagesDeleted = Utility.DeleteDocument(brand.ImgPath);
                    return isBrandUpdated;
                }

                throw new ArgumentException(errorMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("Elemento non cancellabile");
            }
        }
        #endregion

        #region private Methods
        #endregion

    }
}
