using BuildingProjectManagementAPI.Model.Dto;
using Microsoft.AspNetCore.Identity;

namespace BuildingProjectManagementAPI.Model.Repositories
{
    public interface IDocumentRepository
    {
        Task<bool> PostDocument(int documentId, IdentityUser user, DocumentCreationDto documentCreationDto);
    }
}
