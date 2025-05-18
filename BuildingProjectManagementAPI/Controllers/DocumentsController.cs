using BuildingProjectManagementAPI.Model.Dao;
using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.Repositories;
using BuildingProjectManagementAPI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildingProjectManagementAPI.Controllers
{
    [ApiController]
    [Route(ApiStrings.ProjectRoute + "/{projectId:int}" + ApiStrings.DocumentRoute)]
    [Authorize]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentRepository documentService;
        private readonly IUserRepository userService;
        private const string container = ApiStrings.DocumentContainer;

        public DocumentsController(IDocumentRepository documentService, IUserRepository userService)
        {
            this.documentService = documentService;
            this.userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromRoute]int projectId, [FromForm]DocumentCreationDto documentCreationDto)
        {
            var user = await userService.GetUser();
            var document = documentService.GetDocumentByDto(documentCreationDto);

            if (documentCreationDto.DocumentFile is not null)
            {
                var url = await documentService.UploadDocument(container, documentCreationDto.DocumentFile);
                document.DocumentPath = url;
            }

            bool createDocument = await documentService.PostDocument(projectId, user!, document);

            if (!createDocument)
            {
                return BadRequest(ApiStrings.DocumentCreationError);
            }

            return Ok(ApiStrings.DocumentCreated);
        }

        [HttpGet]
        public async Task<ActionResult<List<DocumentDto>>> Get([FromRoute]int projectId)
        {
            return await documentService.GetDocuments(projectId);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var document = await documentService.GetDocumentById(id);

            if (document is null)
            {
                return NotFound(ApiStrings.DocumentNotFound);
            }

            var user = await userService.GetUser();
            bool checkUserDocument = documentService.CheckUserDocument(user!, document);

            if (!checkUserDocument)
            {
                return Forbid();
            }

            bool canDelete = await documentService.DeleteDocument(document);

            if (canDelete)
            {
                await documentService.DeleteDocFromServer(document.DocumentPath, container);
                return Ok(ApiStrings.DocumentDeleted);
            }
            else
            {
                return BadRequest(ApiStrings.DocumentDeleteError);
            }
        }
    }
}
