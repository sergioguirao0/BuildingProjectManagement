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
using System.Threading.Tasks;

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

        public async Task<bool> PostContact(IdentityUser? user, ContactEntity contact)
        {
            try
            {
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

        public async Task<bool> ContactExist(IdentityUser? user, ContactEntity contact)
        {
            var contactExist = await context.Contactos.FirstOrDefaultAsync(c => c.Dni == contact.Dni && c.UserId == user!.Id);

            if (contactExist is null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ContactEntity GetContact(ContactCreationDTO contactCreationDTO)
        {
            return mapper.Map<ContactEntity>(contactCreationDTO);
        }

        public async Task<ContactEntity?> GetContactById(int id)
        {
            return await context.Contactos.FirstOrDefaultAsync(contact => contact.Id == id);
        }

        public async Task<ActionResult<List<ContactDto>>> GetContacts()
        {
            var user = await userService.GetUser();

            var contacts = await context.Contactos
                .Include(contact => contact.User)
                .Where(contact => contact.UserId == user!.Id)
                .OrderBy(contact => contact.Profession)
                .ToListAsync();

            return mapper.Map<List<ContactDto>>(contacts);
        }

        public async Task<bool> PutContact(int id, ContactCreationDTO contactCreationDTO)
        {
            try
            {
                var contact = await context.Contactos.FirstOrDefaultAsync(c => c.Id == id);

                if (contact is null)
                {
                    return false;
                }

                mapper.Map(contactCreationDTO, contact);
                context.Update(contact);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool CheckUserContact(IdentityUser user, ContactEntity? contact)
        {
            if (contact != null)
            {
                if (contact.UserId != user.Id)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteContact(ContactEntity contact)
        {
            try
            {
                context.Remove(contact);
                await context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }
}
