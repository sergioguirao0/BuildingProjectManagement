using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        private readonly string connectionString;

        public DocumentService(ApplicationDbContext context, IMapper mapper, IConfiguration configuration)
        {
            this.context = context;
            this.mapper = mapper;
            this.connectionString = configuration.GetConnectionString("AzureStorageConnection")!;
        }

        public async Task<bool> PostDocument(int projectId, IdentityUser user, DocumentEntity document)
        {
            try
            {
                document.ProjectId = projectId;
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

        public DocumentEntity GetDocumentByDto(DocumentCreationDto documentCreationDto)
        {
            return mapper.Map<DocumentEntity>(documentCreationDto);
        }

        public async Task<string> UploadDocument(string container, IFormFile file)
        {
            try
            {
                var client = new BlobContainerClient(connectionString, container);
                await client.CreateIfNotExistsAsync();
                client.SetAccessPolicy(PublicAccessType.Blob);

                var extension = Path.GetExtension(file.FileName);
                var nameFile = $"{Guid.NewGuid()}{extension}";
                var blob = client.GetBlobClient(nameFile);
                var blobHttpHeaders = new BlobHttpHeaders();
                blobHttpHeaders.ContentType = file.ContentType;
                await blob.UploadAsync(file.OpenReadStream(), blobHttpHeaders);
                return blob.Uri.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return "";
            }
        }

        public async Task DeleteDocument(string? path, string container)
        {
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            var nameFile = Path.GetFileName(path);
            var blob = client.GetBlobClient(nameFile);
            await blob.DeleteIfExistsAsync();
        }
    }
}
