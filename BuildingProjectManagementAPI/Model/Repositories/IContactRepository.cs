using BuildingProjectManagementAPI.Model.Dto;
using Microsoft.AspNetCore.Mvc;

namespace BuildingProjectManagementAPI.Model.Repositories
{
    public interface IContactRepository
    {
        Task<bool> PostContact(int contactId, ContactCreationDTO contactCreationDTO);
    }
}
