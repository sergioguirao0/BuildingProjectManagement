using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.Entities;
using BuildingProjectManagementAPI.Model.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildingProjectManagementAPI.Controllers
{
    [ApiController]
    [Route("api/contacts")]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository contactService;

        public ContactsController(IContactRepository contactService)
        {
            this.contactService = contactService;
        }

        [HttpPost]
        public async Task<ActionResult> Post(int contactId, ContactCreationDTO contactCreationDTO)
        {
            bool postContact = await contactService.PostContact(contactId, contactCreationDTO);

            if (!postContact)
            {
                return BadRequest("Error al crear el contacto");
            }

            return Ok("Contacto creado correctamente");
        }
    }
}
