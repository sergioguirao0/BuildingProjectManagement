using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BuildingProjectManagement.ViewModel
{
    public class ContactViewModel : ViewModelBase
    {
        public ObservableCollection<Contact>? Contacts { get; set; }

        private string? _confirmationWindowTitle;
        public string? ConfirmationWindowTitle
        {
            get => _confirmationWindowTitle;
            set
            {
                if (_confirmationWindowTitle != value)
                {
                    _confirmationWindowTitle = value;
                    OnPropertyChanged(nameof(ConfirmationWindowTitle));
                }
            }
        }

        private string? _confirmationWindowValidation;
        public string? ConfirmationWindowValidation
        {
            get => _confirmationWindowValidation;
            set
            {
                if (_confirmationWindowValidation != value)
                {
                    _confirmationWindowValidation = value;
                    OnPropertyChanged(nameof(ConfirmationWindowValidation));
                }
            }
        }

        private Contact? _selectedContact;
        public Contact? SelectedContact
        {
            get => _selectedContact;
            set
            {
                if (_selectedContact != value)
                {
                    _selectedContact = value;
                    OnPropertyChanged(nameof(SelectedContact));
                }
            }
        }

        public async Task<HttpResponseMessage> GetContactResponse()
        {
            try
            {
                using (var client = GetHttpClient())
                {
                    return await client.GetAsync(AppStrings.ContactsEndpoint);
                }
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

        public async Task ShowContacts()
        {
            var response = await GetContactResponse();

            if (response.IsSuccessStatusCode)
            {
                var contacts = await GetContacts(response);
                Contacts!.Clear();

                foreach (var contact in contacts!)
                {
                    Contacts.Add(contact);
                }
            }
        }

        private bool CheckPhone(string phone)
        {
            if (!string.IsNullOrEmpty(phone))
            {
                string regexPhone = "\\d{9}";
                return Regex.IsMatch(phone, regexPhone);
            }
            else
            {
                return true;
            }
        }

        private bool CheckContactEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return true;
            }
            else
            {
                return CheckEmail(email);
            }
        }

        public bool ContactChecks(string name, string dni, string phone, string email, string profession)
        {
            bool checks;

            if (!CheckName(name))
            {
                CheckMessage = AppStrings.CheckName;
                checks = false;
            }
            else if (!CheckIdentification(dni))
            {
                CheckMessage = AppStrings.CheckDniContact;
                checks = false;
            }
            else if (!CheckPhone(phone))
            {
                CheckMessage = AppStrings.CheckPhone;
                checks = false;
            }
            else if (!CheckContactEmail(email))
            {
                CheckMessage = AppStrings.CheckEmail;
                checks = false;
            }
            else if (string.IsNullOrEmpty(profession))
            {
                CheckMessage = AppStrings.CheckProfession;
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
            CheckMessage = string.Empty;
        }

        public void ValidateContactForm(Contact contact, string address, string town, string province, string phone, string email)
        {
            if (!string.IsNullOrEmpty(address))
                contact.Address = address;

            if (!string.IsNullOrEmpty(town))
                contact.Town = town;

            if (!string.IsNullOrEmpty(province))
                contact.Province = province;

            if (!string.IsNullOrEmpty(phone) && CheckPhone(phone))
                contact.Phone = phone;

            if (!string.IsNullOrEmpty(email) && CheckEmail(email))
                contact.Email = email;
        }

        public async Task<HttpResponseMessage> PostContact(Contact contact)
        {
            try
            {
                using (var client = GetHttpClient())
                {
                    var json = JsonSerializer.Serialize(contact);
                    var content = new StringContent(json, Encoding.UTF8, AppStrings.ApplicationJson);
                    return await client.PostAsync(AppStrings.ContactsEndpoint, content);
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

        public async Task<HttpResponseMessage> PutContact(int id, Contact contact)
        {
            try
            {
                using (var client = GetHttpClient())
                {
                    var json = JsonSerializer.Serialize(contact);
                    var content = new StringContent(json, Encoding.UTF8, AppStrings.ApplicationJson);
                    return await client.PutAsync(AppStrings.ContactsEndpoint + "/" + id, content);
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

        public async Task<HttpResponseMessage> DeleteContact(int id)
        {
            try
            {
                using (var client = GetHttpClient())
                {
                    return await client.DeleteAsync(AppStrings.ContactsEndpoint + "/" + id);
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
