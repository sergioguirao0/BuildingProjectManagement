using AutoMapper;
using BuildingProjectManagementAPI.Data;
using BuildingProjectManagementAPI.Model.Dao;
using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.Entities;
using BuildingProjectManagementAPI.Model.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BuildingProjectManagementAPI.Services
{
    public class ContactService : IContactRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUserRepository userService;

        public ContactService(ApplicationDbContext context, IMapper mapper, IUserRepository userService)
        {
            this.context = context;
            this.mapper = mapper;
            this.userService = userService;
        }

        public async Task<bool> PostContact(int contactId, ContactCreationDTO contactCreationDTO)
        {
            try
            {
                var user = await userService.GetUser();
                var contact = mapper.Map<ContactEntity>(contactCreationDTO);

                var existeContacto = await context.Contactos.FirstOrDefaultAsync(c => c.Dni == contact.Dni && c.UserId == user!.Id);

                if (existeContacto != null)
                {
                    return false;
                }

                contact.Id = contactId;
                contact.UserId = user!.Id;
                context.Add(contact);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
}
