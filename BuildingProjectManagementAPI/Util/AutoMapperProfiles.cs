using AutoMapper;
using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.DTO;
using BuildingProjectManagementAPI.Model.Entities;

namespace BuildingProjectManagementAPI.Util
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserRegisterDTO, UserEntity>()
                .ForMember(ent => ent.Nombre, options => options.MapFrom(user => user.Name))
                .ForMember(ent => ent.Apellidos, options => options.MapFrom(user => user.Surname))
                .ForMember(ent => ent.Dni, options => options.MapFrom(user => user.Dni))
                .ForMember(ent => ent.Email, options => options.MapFrom(user => user.Email))
                .ReverseMap();
        }
    }
}
