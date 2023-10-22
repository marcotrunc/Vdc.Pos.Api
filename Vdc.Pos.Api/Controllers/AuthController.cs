using Microsoft.AspNetCore.Mvc;
using Vdc.Pos.Business.Services;
using Vdc.Pos.Business.Services.Interfaces;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;
using Vdc.Pos.Domain.Entities;
using Vdc.Pos.Infrastructure.Service.Interfaces;

namespace Vdc.Pos.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly IUserServices _userServices;
        private readonly OtpServices _otpServices;
        

        public AuthController(
            IAuthenticationService authenticationService, 
            IEmailService emailService, 
            IUserServices userServices, 
            OtpServices otpServices
            ) 
        {
            _authenticationService = authenticationService;
            _emailService = emailService;
            _userServices = userServices;
            _otpServices = otpServices;
        }
        [HttpPost("register")]
        public async Task<ActionResult<UserAuthResponseDto>> Register(UserRequestDto request)
        {
            try
            {
                var userRegistered = await _authenticationService.Register(request);

                if(userRegistered is null) 
                {
                    return BadRequest($"L'utente non è stato registrato");      
                }

                string message = @$"<p>
                                        Ciao {userRegistered.Name},<br> 
                                        Entra in Vdc POS e clicca 'Primo accesso' in fondo a destra.<br> 
                                        Dopodiché ti sarà possibile settare la password.<br>
                                        Grazie
                                        <br>
                                        <br> 
                                        Team VDC
                                    </p>";


                _emailService.SendEmail("noreply@vdc.it",userRegistered.Email,"Registrazione a Vdc Pos",message);
                
                return Ok(userRegistered); 
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);    
            }
            
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserAuthResponseDto>> Login(UserAuthRequestDto request)
        {
            try
            {
                var userAuthenticated = await _authenticationService.Login(request);

                return Ok(userAuthenticated);
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("updatePassword")]
        public async Task<ActionResult<UserAuthResponseDto>> UpdatePassword(UpdatePasswordModuleRequestDTO model)
        {
            try
            {
                return await _userServices.UpdatePassword(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("otp-validation")]
        public async Task<ActionResult<bool>> IsOtpValidationOvercome([FromQuery] string otp, string email)
        {
            try 
            {
                Guid userId = await _userServices.GetUserGuidFromEmailAsync(email);

                bool isOtpValidated = await _otpServices.IsOtpValidationOvercome(otp, userId);

                if(isOtpValidated == false) 
                {
                    return false;
                }

                var isUserEmailVerified = await _userServices.SetUserMailVerified(userId);

                if(isUserEmailVerified == true)
                {
                    return true;
                }

                throw new Exception("Otp Valido ma utente non aggiornato, contattare l'assistenza");
                
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("send-otp")]
        public async Task<ActionResult<string>> SendAndSaveOtp([FromQuery] string email)
        {
            try
            {
                Guid userId = await _userServices.GetUserGuidFromEmailAsync(email);

                bool isOtpSent = await _otpServices.IsOtpRegisteredAndSent(email, userId);

                if(isOtpSent == true)
                {
                    return Ok($"OTP inviato correttamente al seguente indirizzo email: {email}");
                }
                else
                {
                    return BadRequest("Non è stato possibile inviare il nuovo OTP, controllare la connessione a internet");
                }
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}