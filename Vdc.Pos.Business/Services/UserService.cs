using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Business.Services.Interfaces;
using Vdc.Pos.Business.UnitOfWork;
using Vdc.Pos.Persistence.IRepositories;

namespace Vdc.Pos.Business.Services
{
    public class UserService : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
                _userRepository = userRepository;
                _unitOfWork = unitOfWork;
        }

        #region public Methods
        public async Task<Guid> GetUserGuidFromEmailAsync(string email)
        {
            if (String.IsNullOrEmpty(email)) 
            {
                throw new ArgumentNullException("Email inserita nulla o vuota");
            }
            
            Guid userId = await _userRepository.GetUserIdFromEmailAsync(email);

            if(userId == Guid.Empty) 
            {
                throw new ArgumentNullException("Utente non registrato");
            }
            return userId;
        }
        #endregion
    }
}
