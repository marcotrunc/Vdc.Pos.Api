using AutoMapper;
using Azure.Core;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Business.Services.Interfaces;
using Vdc.Pos.Business.UnitOfWork;
using Vdc.Pos.Business.Utilities;
using Vdc.Pos.Business.Validators;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Persistence.IRepositories;

namespace Vdc.Pos.Business.Services
{
    public class UserService : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdatePasswordModuleRequestDTO> _updatePwdModuleValidator;
        private readonly IMapper _mapper;
        
        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<UpdatePasswordModuleRequestDTO> updatePwdModuleValidator
            )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _updatePwdModuleValidator = updatePwdModuleValidator;
        }

        #region public Methods
        public async Task<Guid> GetUserGuidFromEmailAsync(string email)
        {
            if (String.IsNullOrEmpty(email)) 
            {
                throw new ArgumentNullException("Email inserita nulla o vuota");
            }
            Guid userId;
            try
            {
                 userId = await _userRepository.GetUserIdFromEmailAsync(email);
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("Utente non registrato");
            }
            

            if(userId == Guid.Empty) 
            {
                throw new ArgumentNullException("Utente non registrato");
            }
            return userId;
        }

        public async Task<bool> SetUserMailVerified(Guid userId)
        {
            if(userId == Guid.Empty) 
            {
                return false;
            }

            var user = await _userRepository.GetByPrimaryKeyAsync(userId);
            
            if(user == null) 
            {
                return false;
            }

            user.IsEmailVerified = true;

            bool isUserUpdated = await _unitOfWork.UpdateAsync(user) > 0;

            return isUserUpdated;
        }

        public async Task<UserAuthResponseDto> UpdatePassword(UpdatePasswordModuleRequestDTO updatePasswordModel)
        {
            await _updatePwdModuleValidator.ValidateAndThrowAsync(updatePasswordModel);
            
            var user = await _userRepository.FindByEmailAsync(updatePasswordModel.Email);

            if(user == null)
            {
                throw new NullReferenceException("Nessun Utente registrato con questa mail");
            }

            if (IsUserDisabled(user))
            {
                throw new Exception($"L'utente {user.Name} {user.LastName} non è abilitato.");
            }

            var password = updatePasswordModel.Password;

            Utility.HashString(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordSalt = passwordSalt;
            user.PasswordHash = passwordHash;

            bool isUserUpdated = await _unitOfWork.UpdateAsync(user) > 0;

            if(isUserUpdated == false) 
            {
                throw new Exception("Salvataggio della nouva password non andata a buon fine, rivolgersi all'assistenza o riprovare");
            }

            return _mapper.Map<UserAuthResponseDto>(user);
        }
        #endregion

        #region private Methods
        private bool IsUserDisabled(User user)
        {
            return user.IsDeleted || user.IsEmailVerified == false || user.IsActived == false;
        }
        #endregion
    }
}
