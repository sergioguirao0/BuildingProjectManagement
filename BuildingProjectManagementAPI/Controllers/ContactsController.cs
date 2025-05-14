using BuildingProjectManagementAPI.Model.Dao;
using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.Entities;
using BuildingProjectManagementAPI.Model.Repositories;
using BuildingProjectManagementAPI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildingProjectManagementAPI.Controllers
{
    [ApiController]
    [Route(ApiStrings.ContactRoute)]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository contactService;
        private readonly IUserRepository userService;

        public ContactsController(IContactRepository contactService, IUserRepository userService)
        {
            this.contactService = contactService;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ContactDto>>> Get()
        {
            return await contactService.GetContacts();
        }

        [HttpPost]
        public async Task<ActionResult> Post(ContactCreationDTO contactCreationDTO)
        {
            var user = await userService.GetUser();
            var contact = contactService.GetContact(contactCreationDTO);

            bool contactExist = await contactService.ContactExist(user, contact);

            if (contactExist)
            {
                return Conflict(ApiStrings.ContactExist);
            }

            bool postContact = await contactService.PostContact(user, contact);

            if (!postContact)
            {
                return BadRequest(ApiStrings.ContactCreationError);
            }

            return Ok(ApiStrings.ContactCreated);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ContactCreationDTO contactCreationDTO)
        {
            var contact = await contactService.GetContactById(id);

            if (contact is null)
            {
                return NotFound();
            }

            var user = await userService.GetUser();
            bool checkUserContact = contactService.CheckUserContact(user!, contact);

            if (!checkUserContact)
            {
                return Forbid();
            }

            bool canUpdate = await contactService.PutContact(id, contactCreationDTO);

            if (canUpdate)
            {
                return Ok(ApiStrings.ContactUpdated);
            }
            else
            {
                return BadRequest(ApiStrings.ContactUpdateError);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var contact = await contactService.GetContactById(id);

            if (contact is null)
            {
                return NotFound();
            }

            var user = await userService.GetUser();
            bool checkUserContact = contactService.CheckUserContact(user!, contact);

            if (!checkUserContact)
            {
                return Forbid();
            }

            bool canDelete = await contactService.DeleteContact(contact!);

            if (canDelete)
            {
                return Ok(ApiStrings.ContactDeleted);
            }
            else
            {
                return BadRequest(ApiStrings.ContactDeleteError);
            }
        }
    }
}
