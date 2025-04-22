using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildingProjectManagementAPI.Controllers
{
    [ApiController]
    [Route(ApiStrings.ProjectRoute)]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> Get()
        {

        }

        [HttpPost]
        public async Task<ActionResult> Post()
        {

        }
    }
}
