using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Business.UnitOfWork;
using Vdc.Pos.Business.Utilities;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Infrastructure.Service.Interfaces;
using Vdc.Pos.Persistence.IRepositories;

namespace Vdc.Pos.Business.Services
{
    public class OtpServices
    {
        private readonly IOtpRepository _otpRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        
        byte[] separatorEncodedBytes = Encoding.UTF8.GetBytes("separatore");

        public OtpServices(
            IOtpRepository otpRepository, 
            IUnitOfWork unitOfWork, 
            IEmailService emailService
            )
        {
            _otpRepository = otpRepository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }
        #region publicMethods
        public async Task<bool> IsOtpRegisteredAndSent(string email, Guid userId)
        {
            if (email == null) 
            {
                return false;
            }
            
            bool isconnectedToInternet = Utility.IsConnectedToInternet();

            if (isconnectedToInternet == false)
            {
                return false;
            }

            string otpCode = this.GeneateRandomOtp(5);

            byte[] otpSalt;
            byte[] otpHash;

            Utility.HashString(otpCode,out otpHash,out otpSalt);
            
            Otp otp = new Otp
            {
                OtpCode = otpSalt.Concat(separatorEncodedBytes).Concat(otpHash).ToArray(),
                UserId = userId,
                ExpiredOn = DateTime.UtcNow.AddMinutes(5).ToLocalTime(),
            };

            var otpAdded = await _otpRepository.InsertAsync(otp);

            if (otpAdded is null)
            {
                return false;
            }

            var results = await _unitOfWork.CommitAsync();

            if(results <= 0)
            {
                return false;
            }

            string message =    @$"<div> 
                                                    <p>
                                                        Ciao, <br>
                                                        Usa il codice <strong>{otpCode}</strong> per confermare la tua identità.
                                                        <br>
                                                        <br>
                                                        Buon Lavoro<br>
                                                        Team di VDC
                                                    </p>
                                                </div> 
                                            ";

            try
            {
                _emailService.SendEmail("marcotrunc@gmail.com", email, "VDC Richiesta Codice OTP" , message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);   
                return false;
            }

            return true;
        }

        public async Task<bool> IsOtpValidationOvercome(string otpCodeString, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(otpCodeString))
            {
                throw new ArgumentNullException("Otp Vuoto o Nullo");
            }

            var otp = await _otpRepository.GetLastOptByUserId(userId);

            if(otp == null) 
            {
                throw new ArgumentNullException("Nessun Otp Generato per questo utente");
            }

            if(otp.ExpiredOn < DateTime.UtcNow.ToLocalTime())
            {
                throw new ArgumentNullException("Otp Scaduto");
            }
            
            GetHashAndSaltFromOtpSaved(otp.OtpCode, out byte[] passwordSalt, out byte[] passwordHash);

            bool isOtpValided = IsOtpVerified(otpCodeString, passwordSalt, passwordHash);

            if (isOtpValided)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region private methods
        private string GeneateRandomOtp(int otpLength)
        {
            const string chars = "0123456789";
            var random = new Random();
            var result = new StringBuilder(otpLength);

            for (int i = 0; i < otpLength; i++)
            {
                int index = random.Next(0, chars.Length);
                result.Append(chars[index]);
            }

            return result.ToString();
        }

        private void GetHashAndSaltFromOtpSaved(byte[] otpSaved, out byte[] passwordSalt, out byte[] passwordHash)
        {
            int separatorIndex = this.OtpArrayIndexOf(otpSaved, separatorEncodedBytes);
            if (separatorIndex != -1)
            {
                passwordSalt = new byte[separatorIndex];
                passwordHash = new byte[otpSaved.Length - separatorIndex - separatorEncodedBytes.Length];

                Array.Copy(otpSaved, 0, passwordSalt, 0, separatorIndex);
                Array.Copy(otpSaved, separatorIndex + separatorEncodedBytes.Length, passwordHash, 0, passwordHash.Length);

            }
            else
            {
                passwordHash = new byte[0];
                passwordSalt = new byte[0];
            }
        }

        private bool IsOtpVerified(string otp, byte[] passwordSalt, byte[] passwordHash)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(otp));
                if (computedHash.SequenceEqual(passwordHash))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private int OtpArrayIndexOf(byte[] source, byte[] pattern)
        {
            for (int i = 0; i <= source.Length - pattern.Length; i++)
            {
                bool found = true;
                for (int j = 0; j < pattern.Length; j++)
                {
                    if (source[i + j] != pattern[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                    return i;
            }
            return -1;
        }
        #endregion
    }
}
