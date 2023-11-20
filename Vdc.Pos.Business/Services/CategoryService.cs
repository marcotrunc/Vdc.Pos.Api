using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Business.Services.Interfaces;
using Vdc.Pos.Business.UnitOfWork;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.IRepositories;
using Vdc.Pos.Persistence.Repositories;

namespace Vdc.Pos.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CategoryService(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region public Methods
        public async Task<CategoryResponseDto> GetCategoryByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException("Id Inserito nullo");
            }

            try
            {
                var category = await _categoryRepository.GetByPrimaryKeyAsync(id);

                if (category == null)
                {
                    throw new ArgumentNullException($"L'{id} inserito non corrisponde a nessuna categoria");
                }

                return _mapper.Map<CategoryResponseDto>(category);
            }
            catch (Exception ex)
            {
                throw new Exception("Categoria Non trovata");
            }

        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();

                if (categories.Any() == false)
                {
                    throw new ArgumentNullException($"Nessuna categoria registrata");
                }

                return _mapper.Map<CategoryResponseDto[]>(categories);
            }
            catch (Exception ex)
            {
                throw new Exception($"Nessuna categoria restituita");
            }
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllParentCategoriesAsync()
        {
            try
            {
                var parentCategories = await _categoryRepository.FindAllParentCategory();

                if (parentCategories.Any() == false)
                {
                    throw new ArgumentNullException($"Nessuna categoria registrata");
                }

                return _mapper.Map<CategoryResponseDto[]>(parentCategories);
            }
            catch (Exception ex)
            {
                throw new Exception("Nessuna sotto-categoria recuperata");
            }
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllChildCategoriesAsync()
        {
            try
            {
                var childCategories = await _categoryRepository.FindAllChildCategory();

                if (childCategories.Any() == false)
                {
                    throw new ArgumentNullException($"Nessuna categoria registrata");
                }

                return _mapper.Map<CategoryResponseDto[]>(childCategories);
            }
            catch (Exception ex)
            {
                throw new Exception("Nessuna sotto-categoria recuperata");
            }
        }

        public async Task<CategoryResponseDto> InsertCategoryAsync(CategoryRequestDto categoryToInsert)
        {
            if (categoryToInsert == null)
            {
                throw new ArgumentNullException($"Inserire una categoria non nulla");
            }

            string categoryName = categoryToInsert.Name;

            if (String.IsNullOrEmpty(categoryName) || String.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentNullException($"Nome della categoria inserito non valido");
            }

            Guid parentId = categoryToInsert.ParentId ?? Guid.Empty;

            if (await IsCategoryAlreadyExists(categoryName, parentId) == true)
            {
                throw new Exception($"Il nome della categoria risulta già usato");
            }

            try
            {
                var categoryToInsertMapped = _mapper.Map<Category>(categoryToInsert);

                var categoryAdded = await _categoryRepository.InsertAsync(categoryToInsertMapped);

                bool isCategoryAdded = await _unitOfWork.CommitAsync() > 0;

                if (categoryAdded != null)
                {
                    return _mapper.Map<CategoryResponseDto>(categoryAdded.Entity);
                }
                else
                {
                    throw new Exception($"Categoria non aggiunta");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<CategoryResponseDto> UpdateCategory(Guid id, CategoryRequestDto categoryToUpdate)
        {
            if(id == Guid.Empty || categoryToUpdate == null) 
            {
                throw new ArgumentNullException($"{(id == Guid.Empty ? "Id Inserito non valido" : "Parametri inseriti nulli" )}");
            }

            var category = await _categoryRepository.GetByPrimaryKeyAsync(id);

            if (category is null)
            {
                throw new ArgumentNullException($"Nessuna categoria associata a questo id");
            }

            string categoryName = categoryToUpdate.Name;

            if(String.IsNullOrEmpty(categoryName) || String.IsNullOrWhiteSpace(categoryName))
            {
                throw new ArgumentNullException($"Nome della categoria inserito non valido");
            }

            category.Name = categoryName;

            if(categoryToUpdate.ParentId != null && category.ParentId != categoryToUpdate.ParentId)
            {
                category.ParentId = categoryToUpdate.ParentId;
            }

            Guid parentId = category.ParentId ?? Guid.Empty;


            if (await IsCategoryAlreadyExists(categoryName, parentId) == true)
            {
                throw new Exception($"Il nome della categoria risulta già usato");
            }

            try
            {
                bool isCategoryUpdated = await _unitOfWork.UpdateAsync(category) > 0;
                
                if(isCategoryUpdated == true)
                {
                    return _mapper.Map<CategoryResponseDto>(category);
                }
                else
                {
                    throw new Exception("Categoria non modificata, errore nel salvataggio");
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Categoria non modificata");
            }

        }
        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            try
            {
                if(id == Guid.Empty)
                {
                    throw new ArgumentNullException("Id inserito non valido");
                }

                await _categoryRepository.DeleteByPrimaryKeyAsync(id);

                return await _unitOfWork.CommitAsync() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Elemento non cancellabile");
            }
        }
        #endregion
        #region private Methods
        private async Task<bool> IsCategoryAlreadyExists(string categoryName, Guid parentId)
        {
            bool isCategoryAlreadyExists = false;

            if (parentId != Guid.Empty)
            {
                isCategoryAlreadyExists = await _categoryRepository.IsUniqueCategoryNameForThisParentCategory(categoryName, parentId);
            }
            else
            {
                isCategoryAlreadyExists = await _categoryRepository.IsUniqueParentCategoryName(categoryName);
            }
            return isCategoryAlreadyExists;
        }

        #endregion
    }
}
