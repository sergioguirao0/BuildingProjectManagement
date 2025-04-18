using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuildingProjectManagement.ViewModel
{
    public class ContactViewModel : ViewModelBase
    {
        public async Task<HttpResponseMessage> GetContactResponse(string token)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(AppStrings.ApiBaseUrl);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await client.GetAsync(AppStrings.ContactsEndpoint);
            }
            catch(Exception ex)
            {
                var errorResponse = new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Error: " + ex.Message)
                };
                return errorResponse;
            }
        }

        public async Task<List<Contact>?> GetContacts(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var contacts = JsonSerializer.Deserialize<List<Contact>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return contacts;
        }
    }
}
