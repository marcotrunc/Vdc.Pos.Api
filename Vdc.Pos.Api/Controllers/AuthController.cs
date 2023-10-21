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
        private readonly OtpServices _otpServices;
        

        public AuthController(IAuthenticationService authenticationService, OtpServices otpServices) 
        {
            _authenticationService = authenticationService;
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

                bool isOtpSent = await _otpServices.IsOtpRegisteredAndSent(userRegistered.Email, userRegistered.Id, true);

                if (isOtpSent == true)
                {
                    // Notifica di avvenuto invio Otp per EMail
                }

                return Ok(userRegistered); 
            }
            catch (Exception ex) 
            {
                return BadRequest(ex.Message);    
            }
            
        }
        [HttpPost("send-otp-first-access")]
        public async Task<ActionResult<string>> SendOtpForFirstAccess([FromBody] string email, Guid id)
        {
            try
            {
                bool isOtpSent = await _otpServices.IsOtpRegisteredAndSent(email, id);
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
    }
}