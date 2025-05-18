using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuildingProjectManagementAPI.Model.Repositories
{
    public interface IDocumentRepository
    {
        Task<bool> PostDocument(int projectId, IdentityUser user, DocumentEntity document);
        Task<string> UploadDocument(string container, IFormFile file);
        Task DeleteDocFromServer(string? path, string container);
        DocumentEntity GetDocumentByDto(DocumentCreationDto documentCreationDto);
        Task<ActionResult<List<DocumentDto>>> GetDocuments(int projectId);
        Task<bool> DeleteDocument(DocumentEntity document);
        Task<DocumentEntity?> GetDocumentById(int id);
        bool CheckUserDocument(IdentityUser user, DocumentEntity? document);
    }
}
