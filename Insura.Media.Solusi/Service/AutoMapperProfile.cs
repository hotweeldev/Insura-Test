using AutoMapper;
using Insura.Media.Solusi.Common.Command;
using Insura.Media.Solusi.Common.Dto;
using Insura.Media.Solusi.Models;

namespace Insura.Media.Solusi.Service
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserCommand, Users>();
            CreateMap<UserDto, Users>()
                .ReverseMap();
            CreateMap<CreateTaskCommand, UserTask>();
        }
    }
}
