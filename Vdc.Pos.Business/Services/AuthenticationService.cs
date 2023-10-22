using System.Text;
using System.Security.Cryptography;
using Vdc.Pos.Business.Services.Interfaces;
using Vdc.Pos.Domain.DTOs.Response;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.Entities;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Vdc.Pos.Persistence.IRepositories;
using Vdc.Pos.Business.UnitOfWork;
using FluentValidation;
using AutoMapper;
using Vdc.Pos.Business.Validators;
using Vdc.Pos.Business.Utilities;
using Utility = Vdc.Pos.Business.Utilities.Utility;

namespace Vdc.Pos.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<UserRequestDto> _userRequestValidator;

        public AuthenticationService(
            IConfiguration configuration,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<UserRequestDto> userRequestValidator
            )
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRequestValidator = userRequestValidator;
        }

        #region public methods
        public async Task<UserAuthResponseDto> Register(UserRequestDto request)
        {

            if (request is null) 
            {
                throw new NullReferenceException("Nessun elemento inviato");
            }
            
            await _userRequestValidator.ValidateAndThrowAsync(request);

            var user = _mapper.Map<User>(request);

            var userAdded = await _userRepository.InsertAsync(user);

            if ( userAdded is null ) 
            {
                throw new NullReferenceException("Salvataggio non riuscito");
            }

            var results = await _unitOfWork.CommitAsync();

            if(results <= 0)
            {
                throw new Exception("Salvataggio non riuscito");
            }

            return _mapper.Map<UserAuthResponseDto>(userAdded.Entity);
        }
        public async Task<UserAuthResponseDto> Login(UserAuthRequestDto request)
        {
            if (request is null)
            {
                throw new NullReferenceException("Nessun elemento inviato");
            }

            bool usernameIsEmail = CustomValidator.IsEmail(request.Username);

            User? user = null;
            if (usernameIsEmail == true)
            {
                user = await _userRepository.FindByEmailAsync(request.Username);
            }
            else
            {
                user = await _userRepository.FindByPhoneNumberAsync(request.Username);
            }

            if (user == null)
            {
                throw new NullReferenceException("Nessun elemento Utente corrispondente allo username inserito");
            }

            if (IsPassWordHashVerified(request.Password, user.PasswordHash, user.PasswordSalt) == false)
            {
                throw new Exception("Password Errata");
            }

            if(IsUserDisabled(user))
            {
                throw new Exception($"L' utente {request.Username} non è abilitato.");
            }
            string accessToken = this.CreateToken(user);

            var userLogged = _mapper.Map<UserAuthResponseDto>(user);
            userLogged.AuthToken = accessToken;
            
            return userLogged;
        }

        //public async Task<UserAuthResponseDto> SetPasswordFromOtp(Guid userId,string newPassword, string confirmedNewPassword)
        //{

        //    //Controllo se le password sono uguali
        //    if(String.IsNullOrEmpty(newPassword) || String.IsNullOrEmpty(confirmedNewPassword))
        //    {
        //        throw new Exception($"Password non valide");
        //    }

        //    if(newPassword != confirmedNewPassword)
        //    {
        //        throw new Exception("Le email inserite non concidono");
        //    }
            
            
        //    // Recupero l'utente associato all'otp


        //    //Update delle password

        //    //Mappo Da utente a Response

        //}
        #endregion

        #region private methods

        private bool IsPassWordHashVerified(string passwordRequest, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac =new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordRequest));
                if(computedHash.SequenceEqual(passwordHash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber ?? ""),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:ClientSecret"] ?? ""));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(Convert.ToDouble(_configuration["Authentication:ExpiresFromInHour"])),
                signingCredentials: credentials
                );
            
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            if (String.IsNullOrEmpty(accessToken))
            {
                return String.Empty;
            }
            else
            {
                return accessToken;
            }
        }

        private bool IsUserDisabled(User user)
        {
            return user.IsDeleted || user.IsEmailVerified == false || user.IsActived == false;
        }

        private string GenerateRandomPassword(int passwordLength)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var result = new StringBuilder(passwordLength);

            for (int i = 0; i < passwordLength; i++)
            {
                int index = random.Next(0, chars.Length);
                result.Append(chars[index]);
            }

            return result.ToString();
        }

        #endregion
    }
}
