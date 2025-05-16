using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace BuildingProjectManagement.ViewModel
{
    public class DocumentViewModel : ViewModelBase
    {
        public ObservableCollection<ProjectDocument> ProjectDocumentList { get; set; } = new ObservableCollection<ProjectDocument>();
        public ObservableCollection<ProjectDocument> StartingDocumentList { get; set; } = new ObservableCollection<ProjectDocument>();
        public ObservableCollection<ProjectDocument> ExecutionDocumentList { get; set; } = new ObservableCollection<ProjectDocument>();
        public ObservableCollection<ProjectDocument> EndingDocumentList { get; set; } = new ObservableCollection<ProjectDocument>();
        public ObservableCollection<ProjectDocument> OtherDocumentList { get; set; } = new ObservableCollection<ProjectDocument>();

        public async Task<HttpResponseMessage> GetProjectDocumentsResponse(int projectId)
        {
            try
            {
                using (var client = GetHttpClient())
                {
                    return await client.GetAsync(AppStrings.ProjectEndpoint + "/" + projectId + AppStrings.DocumentEndpoint);
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

        public async Task<List<ProjectDocument>> GetProjectDocuments(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var documents = JsonSerializer.Deserialize<List<ProjectDocument>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (documents is not null)
            {
                return documents;
            }
            else
            {
                return new List<ProjectDocument>();
            }
        }

        public void OrderDocuments(List<ProjectDocument> documents)
        {
            if (documents is not null)
            {
                ProjectDocumentList.Clear();
                StartingDocumentList.Clear();
                ExecutionDocumentList.Clear();
                EndingDocumentList.Clear();
                OtherDocumentList.Clear();

                foreach (var document in documents)
                {
                    var collection = document.Category switch
                    {
                        AppStrings.ProjectCategory => ProjectDocumentList,
                        AppStrings.StartingProjectCategory => StartingDocumentList,
                        AppStrings.ExecutionProjectCategory => ExecutionDocumentList,
                        AppStrings.EndingProjectCategory => EndingDocumentList,
                        _ => OtherDocumentList
                    };

                    collection.Add(document);
                }
            }
        }
    }
}
