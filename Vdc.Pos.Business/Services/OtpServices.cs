using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<bool> IsOtpRegisteredAndSent(string email, Guid userId,bool isRegisterPhase = false)
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

            byte[] otpEncodedBytes = Encoding.UTF8.GetBytes(otpCode);

            Otp otp = new Otp
            {
                OtpCode = otpEncodedBytes,
                UserId = userId,
                ExpiredOn = isRegisterPhase ? DateTime.UtcNow.AddDays(3) : DateTime.UtcNow.AddMinutes(5).ToLocalTime(),
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

            string messageRegisterPhase =    @$"<div> 
                                                    <p>
                                                        Ciao, </br>
                                                        Una volta aperta la pagina di Login di VDC POS </br>
                                                        Clicca in basso a destra  su Primo Accesso. </br>
                                                        Inserisci la mail, che nel tuo caso è: <strong>{email}</strong>.</br>
                                                        Il codice per poter procedere alla fase di autenticazione è <strong>{otpCode}</strong> in scadenza il {otpAdded.Entity.ExpiredOn}.</br>
                                                        In seguito sarà possibile impostare la password.</br>
                                                        </br>
                                                        </br>
                                                        Buon Lavoro</br>
                                                        Team di VDC
                                                    </p>
                                                </div> 
                                            ";

            try
            {
                _emailService.SendEmail("marcotrunc@gmail.com", email, isRegisterPhase ? "Primo Accesso VDC POS" : "Codice OTP" , isRegisterPhase ? messageRegisterPhase : $"Il codice per poter procedere è {otpCode}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);   
                return false;
            }

            return true;
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
        #endregion
    }
}
