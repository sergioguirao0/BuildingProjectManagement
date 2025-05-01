using AutoMapper;
using BuildingProjectManagementAPI.Model.Dao;
using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.Repositories;
using BuildingProjectManagementAPI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BuildingProjectManagementAPI.Controllers
{
    [ApiController]
    [Route(ApiStrings.ProjectRoute)]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IUserRepository userService;
        private readonly IProjectRepository projectService;

        public ProjectsController(IUserRepository userService, IProjectRepository projectService)
        {
            this.userService = userService;
            this.projectService = projectService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> Get()
        {
            return await projectService.GetProjects();
        }

        [HttpPost]
        public async Task<ActionResult> Post(ProjectCreationDto projectCreationDto)
        {
            var user = await userService.GetUser();
            bool checkContactExist = await projectService.CheckContactExist(user!, projectCreationDto);

            if (!checkContactExist)
            {
                ModelState.AddModelError(nameof(projectCreationDto.ContactsIds), ApiStrings.ProjectContactExist);
                return ValidationProblem();
            }

            bool postProject = await projectService.PostProject(user!, projectCreationDto);

            if (!postProject)
            {
                return BadRequest(ApiStrings.ProjectCreationError);
            }

            return Ok(ApiStrings.ProjectCreated);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<ProjectPatchDto> patchDoc)
        {
            if (patchDoc is null)
            {
                return BadRequest();
            }

            var projectDb = await projectService.GetProject(id);

            if (projectDb is null)
            {
                return NotFound(ApiStrings.ProjectNotFound);
            }

            var user = await userService.GetUser();
            bool checkUserProject = projectService.CheckUserProject(user!, projectDb);

            if (!checkUserProject)
            {
                return Forbid();
            }

            var projectPatchDto = projectService.MapProject(projectDb);
            patchDoc.ApplyTo(projectPatchDto, ModelState);
            var projectValid = TryValidateModel(projectPatchDto);

            if (!projectValid)
            {
                return ValidationProblem();
            }

            await projectService.PatchProject(projectDb, projectPatchDto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var project = await projectService.GetProject(id);

            if (project is null)
            {
                return NotFound(ApiStrings.ProjectNotFound);
            }

            var user = await userService.GetUser();
            bool checkProjectContact = projectService.CheckUserProject(user!, project);

            if (!checkProjectContact)
            {
                return Forbid();
            }

            bool canDelete = await projectService.DeleteProject(project);

            if (canDelete)
            {
                return Ok(ApiStrings.ProjectDeleted);
            }
            else
            {
                return BadRequest(ApiStrings.ProjectDeleteError);
            }
        }
    }
}
