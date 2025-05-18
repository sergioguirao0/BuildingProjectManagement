using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        private DocumentPost? _documentToUpload;
        public DocumentPost? DocumentToUpload
        {
            get => _documentToUpload;
            set
            {
                if (_documentToUpload != value)
                {
                    _documentToUpload = value;
                    OnPropertyChanged(nameof(DocumentToUpload));
                }
            }
        }

        private string? _documentChecksMessage;
        public string? DocumentChecksMessage
        {
            get => _documentChecksMessage;
            set
            {
                if (_documentChecksMessage != value)
                {
                    _documentChecksMessage = value;
                    OnPropertyChanged(nameof(DocumentChecksMessage));
                }
            }
        }

        private ProjectDocument? _selectedDocument;
        public ProjectDocument? SelectedDocument
        {
            get => _selectedDocument;
            set
            {
                if (_selectedDocument != value)
                {
                    _selectedDocument = value;
                    OnPropertyChanged(nameof(SelectedDocument));
                }
            }
        }

        public DocumentViewModel()
        {
            DocumentToUpload = new DocumentPost(string.Empty, string.Empty, string.Empty);
        }

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

        public bool CheckDocument(string title, string category, string filePath)
        {
            bool checks;

            if (string.IsNullOrEmpty(title))
            {
                checks = false;
                DocumentChecksMessage = AppStrings.NoTitleError;
            }
            else if (string.IsNullOrEmpty(category))
            {
                checks = false;
                DocumentChecksMessage = AppStrings.NoCategoryError;
            }
            else if (string.IsNullOrEmpty(filePath))
            {
                checks = false;
                DocumentChecksMessage = AppStrings.NoFilePathError;
            }
            else
            {
                checks = true;
                DocumentChecksMessage = string.Empty;
            }

            return checks;
        }

        public async Task<HttpResponseMessage> PostDocument(DocumentPost document, int projectId)
        {
            try
            {
                using (var client = GetHttpClient())
                {
                    document.ProjectId = projectId;
                    var form = new MultipartFormDataContent();
                    var fileStream = new FileStream(document.DocumentPath, FileMode.Open, FileAccess.Read);
                    var streamContent = new StreamContent(fileStream);
                    streamContent.Headers.ContentType = new MediaTypeHeaderValue(AppStrings.ApplicationPdf);
                    var fileName = Path.GetFileName(document.DocumentPath);

                    form.Add(new StringContent(document.Title), "Title");
                    form.Add(new StringContent(document.Category), "Category");
                    form.Add(streamContent, "DocumentFile", fileName);
                    form.Add(new StringContent(document.ProjectId.ToString()!), "ProjectId");

                    return await client.PostAsync(AppStrings.ProjectEndpoint + "/" + projectId + AppStrings.DocumentEndpoint, form);
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

        public async Task<HttpResponseMessage> DeleteDocument(int documentId, int projectId)
        {
            try
            {
                using (var client = GetHttpClient())
                {
                    return await client.DeleteAsync(AppStrings.ProjectEndpoint + "/" + projectId + AppStrings.DocumentEndpoint + "/" + documentId);
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
