using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
using BuildingProjectManagement.Views;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuildingProjectManagement.ViewModel
{
    public class ProjectViewModel : ViewModelBase
    {
        private string? _projectChecksMessage;
        public string? ProjectChecksMessage
        {
            get => _projectChecksMessage;
            set
            {
                if (_projectChecksMessage != value)
                {
                    _projectChecksMessage = value;
                    OnPropertyChanged(nameof(ProjectChecksMessage));
                }
            }
        }

        private string? _newProjectChecksMessage;
        public string? NewProjectChecksMessage
        {
            get => _newProjectChecksMessage;
            set
            {
                if (_newProjectChecksMessage != value)
                {
                    _newProjectChecksMessage = value;
                    OnPropertyChanged(nameof(NewProjectChecksMessage));
                }
            }
        }

        private string? _projectDocumentMessage;
        public string? ProjectDocumentMessage
        {
            get => _projectDocumentMessage;
            set
            {
                if (_projectDocumentMessage != value)
                {
                    _projectDocumentMessage = value;
                    OnPropertyChanged(nameof(ProjectDocumentMessage));
                }
            }
        }

        private ObservableCollection<Project>? _projects;
        public ObservableCollection<Project>? Projects
        {
            get => _projects;
            set
            {
                if (_projects != value)
                {
                    _projects = value;
                    OnPropertyChanged(nameof(Projects));
                }
            }
        }

        private Project? _selectedProject;
        public Project? SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (_selectedProject != value)
                {
                    _selectedProject = value;
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        private string? _confirmationTitle;
        public string? ConfirmationTitle
        {
            get => _confirmationTitle;
            set
            {
                if (_confirmationTitle != value)
                {
                    _confirmationTitle = value;
                    OnPropertyChanged(nameof(ConfirmationTitle));
                }
            }
        }

        private string? _confirmationMessage;
        public string? ConfirmationMessage
        {
            get => _confirmationMessage;
            set
            {
                if (_confirmationMessage != value)
                {
                    _confirmationMessage = value;
                    OnPropertyChanged(nameof(ConfirmationMessage));
                }
            }
        }

        public ObservableCollection<Contact>? ProjectContacts { get; set; }

        public bool ProjectChecks(string name, string site, string jobType)
        {
            bool checks;

            if (string.IsNullOrEmpty(name))
            {
                NewProjectChecksMessage = AppStrings.CheckName;
                checks = false;
            }
            else if (string.IsNullOrEmpty(site))
            {
                NewProjectChecksMessage = AppStrings.CheckSite;
                checks = false;
            }
            else if (string.IsNullOrEmpty(jobType))
            {
                NewProjectChecksMessage = AppStrings.CheckJobType;
                checks = false;
            }
            else
            {
                NewProjectChecksMessage = string.Empty;
                checks = true;
            }
            
            return checks;
        }

        public void CleanCheckMessage()
        {
            ProjectChecksMessage = string.Empty;
        }

        public void ValidateProjectForm(ProjectPost project, string description)
        {
            if (!string.IsNullOrEmpty(description))
                project.Description = description;
        }

        public async Task<HttpResponseMessage> PostProject(ProjectPost project)
        {
            try
            {
                using (var client = GetHttpClient())
                {
                    var json = System.Text.Json.JsonSerializer.Serialize(project);
                    var content = new StringContent(json, Encoding.UTF8, AppStrings.ApplicationJson);
                    return await client.PostAsync(AppStrings.ProjectEndpoint, content);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Error: " + ex.Message)
                };
                return errorResponse;
            }
        }

        public async Task<HttpResponseMessage> GetProjectsResponse()
        {
            try
            {
                using (var client = GetHttpClient())
                {
                    return await client.GetAsync(AppStrings.ProjectEndpoint);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Error: " + ex.Message)
                };
                return errorResponse;
            }
        }

        public async Task<List<Project>?> GetProjects(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var projects = System.Text.Json.JsonSerializer.Deserialize<List<Project>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return projects;
        }

        public async Task ShowProjects()
        {
            var response = await GetProjectsResponse();

            if (response.IsSuccessStatusCode)
            {
                var projects = await GetProjects(response);

                if (projects is not null)
                {
                    if (Projects is null)
                        Projects = new ObservableCollection<Project>();

                    Projects!.Clear();

                    foreach (var project in projects!)
                    {
                        Projects.Add(project);
                    }
                }
                else
                {
                    Projects = new ObservableCollection<Project>();
                }
            }
        }

        public async Task<HttpResponseMessage> DeleteProject(int id)
        {
            try
            {
                using (var client = GetHttpClient())
                {
                    return await client.DeleteAsync(AppStrings.ProjectEndpoint + "/" + id);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Error: " + ex.Message)
                };
                return errorResponse;
            }
        }

        public List<int> GetListContactsIds(List<Contact> contacts)
        {
            List<int> contactsIds = new List<int>();

            foreach (var contact in contacts)
            {
                contactsIds.Add(contact.Id);
            }

            return contactsIds;
        }

        public JsonPatchDocument GetPatchDoc(Project originalProject, Project updatedProject)
        {
            var patchDoc = new JsonPatchDocument();

            if (updatedProject.Name != originalProject.Name)
                patchDoc.Replace("/name", updatedProject.Name);

            if (updatedProject.Site != originalProject.Site)
                patchDoc.Replace("/site", updatedProject.Site);

            if (updatedProject.JobType != originalProject.JobType)
                patchDoc.Replace("/jobType", updatedProject.JobType);

            if (updatedProject.Description != originalProject.Description)
                patchDoc.Replace("/description", updatedProject.Description);

            List<int> contactsIds = GetListContactsIds(updatedProject.Contacts!);
            patchDoc.Replace("/contactsIds", contactsIds);

            if (updatedProject.State != originalProject.State)
                patchDoc.Replace("/state", updatedProject.State);

            return patchDoc;
        }

        public async Task<HttpResponseMessage> PatchProject(int id, Project original, Project updated)
        {
            try
            {
                using (var client = GetHttpClient())
                {
                    var patchDoc = GetPatchDoc(original, updated);
                    var json = JsonConvert.SerializeObject(patchDoc);
                    var content = new StringContent(json, Encoding.UTF8, AppStrings.ApplicationJson);
                    var request = new HttpRequestMessage(HttpMethod.Patch, AppStrings.ProjectEndpoint + "/" + id) { Content = content };
                    return await client.SendAsync(request);
                }
            }
            catch (Exception ex)
            {
                var errorResponse = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Error: " + ex.Message)
                };
                return errorResponse;
            }
        }
    }
}
