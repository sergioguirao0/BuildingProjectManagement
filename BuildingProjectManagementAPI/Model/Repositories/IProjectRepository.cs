using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuildingProjectManagementAPI.Model.Repositories
{
    public interface IProjectRepository
    {
        Task<bool> PostProject(IdentityUser user, ProjectCreationDto projectCreationDto);
        Task<bool> CheckContactExist(IdentityUser user, ProjectCreationDto projectCreationDto);
        Task<ActionResult<List<ProjectDto>>> GetProjects();
        Task<ProjectEntity?> GetProject(int id);
        ProjectPatchDto MapProject(ProjectEntity project);
        Task<bool> PatchProject(ProjectEntity project, ProjectPatchDto projectPatchDto);
        bool CheckUserProject(IdentityUser user, ProjectEntity? project);
        Task<bool> DeleteProject(ProjectEntity project);
    }
}
