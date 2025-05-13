using AutoMapper;
using BuildingProjectManagementAPI.Data;
using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.Entities;
using BuildingProjectManagementAPI.Model.Repositories;
using Microsoft.AspNetCore.Identity;

namespace BuildingProjectManagementAPI.Services
{
    public class DocumentService : IDocumentRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public DocumentService(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<bool> PostDocument(int documentId, IdentityUser user, DocumentCreationDto documentCreationDto)
        {
            try
            {
                var document = mapper.Map<DocumentEntity>(documentCreationDto);
                document.Id = documentId;
                document.UserId = user.Id;
                context.Add(document);
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
