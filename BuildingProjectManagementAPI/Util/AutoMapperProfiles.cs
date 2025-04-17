using AutoMapper;
using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.DTO;
using BuildingProjectManagementAPI.Model.Entities;
using Microsoft.AspNetCore.Identity;

namespace BuildingProjectManagementAPI.Util
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserRegisterDTO, UserEntity>()
                .ForMember(ent => ent.Name, options => options.MapFrom(user => user.Name))
                .ForMember(ent => ent.Surname, options => options.MapFrom(user => user.Surname))
                .ForMember(ent => ent.Dni, options => options.MapFrom(user => user.Dni))
                .ForMember(ent => ent.Email, options => options.MapFrom(user => user.Email))
                .ReverseMap();

            CreateMap<ContactCreationDTO, ContactEntity>()
                .ForMember(ent => ent.Name, options => options.MapFrom(user => user.Name))
                .ForMember(ent => ent.Dni, options => options.MapFrom(user => user.Dni))
                .ForMember(ent => ent.Address, options => options.MapFrom(user => user.Address))
                .ForMember(ent => ent.Town, options => options.MapFrom(user => user.Town))
                .ForMember(ent => ent.Province, options => options.MapFrom(user => user.Province))
                .ForMember(ent => ent.Phone, options => options.MapFrom(user => user.Phone))
                .ForMember(ent => ent.Email, options => options.MapFrom(user => user.Email))
                .ForMember(ent => ent.Profession, options => options.MapFrom(user => user.Profession))
                .ReverseMap();
        }
    }
}
