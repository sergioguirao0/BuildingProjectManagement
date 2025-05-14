using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuildingProjectManagementAPI.Model.Repositories
{
    public interface IContactRepository
    {
        Task<bool> PostContact(IdentityUser? user, ContactEntity contact);
        Task<bool> ContactExist(IdentityUser? user, ContactEntity contact);
        ContactEntity GetContact(ContactCreationDTO contactCreationDTO);
        Task<ContactEntity?> GetContactById(int id);
        Task<ActionResult<List<ContactDto>>> GetContacts();
        Task<bool> PutContact(int id, ContactCreationDTO contactCreationDTO);
        bool CheckUserContact(IdentityUser user, ContactEntity? contact);
        Task<bool> DeleteContact(ContactEntity contact);
    }
}
