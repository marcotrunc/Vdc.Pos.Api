using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vdc.Pos.Domain.DTOs.Requests;
using Vdc.Pos.Domain.DTOs.Response;
using Vdc.Pos.Domain.Entities;

namespace Vdc.Pos.Business.Mappers
{
    public sealed class UserMapperProfiler : Profile
    {
        public UserMapperProfiler() 
        {
            CreateMap<UserRequestDto, User>();
            CreateMap<User, UserAuthResponseDto>();
        }
    }
}
