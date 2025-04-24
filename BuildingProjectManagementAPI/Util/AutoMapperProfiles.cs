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

            CreateMap<ContactEntity, ContactDto>()
                .ForMember(ent => ent.UserEmail, options => options.MapFrom(user => user.User!.Email))
                .ReverseMap();

            CreateMap<ProjectCreationDto, ProjectEntity>()
                .ForMember(ent => ent.Contacts, options => 
                    options.MapFrom(dto => dto.ContactsIds.Select(id => new ProjectContactEntity { ContactId = id })));

            CreateMap<ProjectEntity, ProjectDto>()
                .ForMember(dto => dto.UserEmail, options => options.MapFrom(project => project.User!.Email));

            CreateMap<ProjectContactEntity, ContactDto>()
                .ForMember(dto => dto.Id, options => options.MapFrom(ent => ent.ContactId))
                .ForMember(dto => dto.Name, options => options.MapFrom(ent => ent.Contact!.Name))
                .ForMember(dto => dto.Dni, options => options.MapFrom(ent => ent.Contact!.Dni))
                .ForMember(dto => dto.Address, options => options.MapFrom(ent => ent.Contact!.Address))
                .ForMember(dto => dto.Town, options => options.MapFrom(ent => ent.Contact!.Town))
                .ForMember(dto => dto.Province, options => options.MapFrom(ent => ent.Contact!.Province))
                .ForMember(dto => dto.Phone, options => options.MapFrom(ent => ent.Contact!.Phone))
                .ForMember(dto => dto.Email, options => options.MapFrom(ent => ent.Contact!.Email))
                .ForMember(dto => dto.Profession, options => options.MapFrom(ent => ent.Contact!.Profession));

            CreateMap<ProjectEntity, ProjectPatchDto>().ReverseMap();
        }
    }
}
