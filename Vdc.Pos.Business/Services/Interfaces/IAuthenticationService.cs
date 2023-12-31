﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;

namespace Vdc.Pos.Business.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserAuthResponseDto> Login(UserAuthRequestDto request);
        Task<UserAuthResponseDto> Register(UserRequestDto request);
    }
}
