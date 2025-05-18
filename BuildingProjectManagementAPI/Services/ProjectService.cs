using AutoMapper;
using BuildingProjectManagementAPI.Data;
using BuildingProjectManagementAPI.Model.Dao;
using BuildingProjectManagementAPI.Model.Dto;
using BuildingProjectManagementAPI.Model.Entities;
using BuildingProjectManagementAPI.Model.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuildingProjectManagementAPI.Services
{
    public class ProjectService : IProjectRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IUserRepository userService;
        private readonly IContactRepository contactService;

        public ProjectService(ApplicationDbContext context, IMapper mapper, IUserRepository userService, IContactRepository contactService)
        {
            this.context = context;
            this.mapper = mapper;
            this.userService = userService;
            this.contactService = contactService;
        }

        public async Task<bool> PostProject(IdentityUser user, ProjectCreationDto projectCreationDto)
        {
            try
            {
                var project = mapper.Map<ProjectEntity>(projectCreationDto);
                project.UserId = user.Id;
                context.Add(project);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> CheckContactExist(IdentityUser user, ProjectCreationDto projectCreationDto)
        {
            var contactsIdExist = await context.Contactos
                    .Where(c => projectCreationDto.ContactsIds
                    .Contains(c.Id)).ToListAsync();

            if (contactsIdExist.Count != projectCreationDto.ContactsIds.Count)
            {
                return false;
            }

            foreach (var contact in contactsIdExist)
            {
                if (user.Id != contact.UserId)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<ActionResult<List<ProjectDto>>> GetProjects()
        {
            var user = await userService.GetUser();

            var projects = await context.Proyectos
                .Include(project => project.User)
                .Include(project => project.Contacts)
                    .ThenInclude(ent => ent.Contact)
                .Include(project => project.Documents)
                .Where(project => project.UserId == user!.Id)
                .ToListAsync();

            return mapper.Map<List<ProjectDto>>(projects);
        }

        public async Task<ProjectEntity?> GetProject(int id)
        {
            return await context.Proyectos
                .Include(project => project.Documents)
                .FirstOrDefaultAsync(project => project.Id == id);
        }

        public ProjectPatchDto MapProject(ProjectEntity project)
        {
            return mapper.Map<ProjectPatchDto>(project);
        }

        public async Task<bool> PatchProject(ProjectEntity project, ProjectPatchDto projectPatchDto)
        {
            try
            {
                mapper.Map(projectPatchDto, project);

                if (projectPatchDto.ContactsIds != null)
                {
                    var projectContacts = await context.ProyectosContactos.Where(ent => ent.ProjectId == project.Id).ToListAsync();
                    context.ProyectosContactos.RemoveRange(projectContacts);

                    if (projectPatchDto.ContactsIds.Any())
                    {
                        var user = await userService.GetUser();

                        var contactsList = await context.Contactos.Where(contact => projectPatchDto.ContactsIds.Contains(contact.Id) &&
                            contact.UserId == user!.Id)
                            .Select(contact => contact.Id)
                            .ToListAsync();

                        var newProjectContacts = contactsList.Select(contactId => new ProjectContactEntity
                        {
                            ProjectId = project.Id,
                            ContactId = contactId
                        }).ToList();

                        await context.ProyectosContactos.AddRangeAsync(newProjectContacts);
                    }
                }

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool CheckUserProject(IdentityUser user, ProjectEntity? project)
        {
            if (project != null)
            {
                if (project.UserId != user.Id)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteProject(ProjectEntity project)
        {
            try
            {
                context.Remove(project);
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
