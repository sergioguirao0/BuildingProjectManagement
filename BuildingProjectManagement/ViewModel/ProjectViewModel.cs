using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
using BuildingProjectManagement.Views;
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

        public ObservableCollection<Contact>? ProjectContacts { get; set; }

        public bool ProjectChecks(string name, string site, string jobType)
        {
            bool checks;

            if (string.IsNullOrEmpty(name))
            {
                ProjectChecksMessage = AppStrings.CheckName;
                checks = false;
            }
            else if (string.IsNullOrEmpty(site))
            {
                ProjectChecksMessage = AppStrings.CheckSite;
                checks = false;
            }
            else if (string.IsNullOrEmpty(jobType))
            {
                ProjectChecksMessage = AppStrings.CheckJobType;
                checks = false;
            }
            else
            {
                CleanCheckMessage();
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
                    var json = JsonSerializer.Serialize(project);
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
            var projects = JsonSerializer.Deserialize<List<Project>>(responseBody, new JsonSerializerOptions
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
    }
}
