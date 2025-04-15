using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BuildingProjectManagement.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        private string? _registerMessage;
        public string? RegisterMessage
        {
            get => _registerMessage;
            set
            {
                if (_registerMessage != value)
                {
                    _registerMessage = value;
                    OnPropertyChanged(nameof(RegisterMessage));
                }
            }
        }

        private string? _checkMessage;
        public string? CheckMessage
        {
            get => _checkMessage;
            set
            {
                if (_checkMessage != value)
                {
                    _checkMessage = value;
                    OnPropertyChanged(nameof(CheckMessage));
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

        public async Task Register(User user)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(AppStrings.ApiBaseUrl);

                var json = JsonSerializer.Serialize(user);
                var content = new StringContent(json, Encoding.UTF8, AppStrings.ApplicationJson);

                var response = await client.PostAsync(AppStrings.RegisterEndpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    ConfirmationMessage = AppStrings.ConfirmationMessage;
                    RegisterMessage = AppStrings.RegisterSuccess;
                }
                else
                {
                    ConfirmationMessage = AppStrings.ErrorMessage;
                    RegisterMessage = AppStrings.RegisterFail;
                }
            }
            catch (Exception ex)
            {
                RegisterMessage = ex.Message;
            }
        }

        public string ChangeFirstChar(string text)
        {
            string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                if (word.Length > 0)
                {
                    result.Append(char.ToUpper(word[0]) + word.Substring(1));
                }

                if (i != word.Length - 1)
                {
                    result.Append(' ');
                }
            }

            return result.ToString();
        }

        private bool CheckName(string name)
        {
            return !string.IsNullOrEmpty(name);
        }

        private bool CheckDni(string dni)
        {
            if (dni.Length != 9)
            {
                return false;
            }
            else
            {
                string regexDni = "^\\d{8}[A-Z]$";
                string regexNie = "^[XYZ]\\d{7}[A-Z]$";

                return Regex.IsMatch(dni, regexDni) || Regex.IsMatch(dni, regexNie);
            }
        }

        private bool CheckEmail(string email)
        {
            string regexEmail = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, regexEmail);
        }

        private bool CheckPassword(string password)
        {
            string regexPassword = "^(?=.*[a-z])(?=.*[A-Z])(?=.*[\\W_]).{8,}$";
            return Regex.IsMatch(password, regexPassword);
        }

        public bool CheckUserData(string name, string surname, string dni, string email, string password, string repeatPassword)
        {
            bool checks;

            if (!CheckName(name) || !CheckName(surname))
            {
                CheckMessage = AppStrings.CheckName;
                checks = false;
            }
            else if (!CheckDni(dni))
            {
                CheckMessage = AppStrings.CheckDni;
                checks = false;
            }
            else if (!CheckEmail(email))
            {
                CheckMessage = AppStrings.CheckEmail;
                checks = false;
            }
            else if (!CheckPassword(password))
            {
                CheckMessage = AppStrings.CheckPasswordFormat;
                checks = false;
            }
            else if (password != repeatPassword)
            {
                CheckMessage = AppStrings.CheckPassword;
                checks = false;
            }
            else
            {
                checks = true;
            }

            return checks;
        }

        public async Task<HttpResponseMessage> LoginUser(UserCredentialsDTO userCredentialsDTO)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(AppStrings.ApiBaseUrl);

                var json = JsonSerializer.Serialize(userCredentialsDTO);
                var content = new StringContent(json, Encoding.UTF8, AppStrings.ApplicationJson);

                return await client.PostAsync(AppStrings.LoginEndpoint, content);
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

        public async Task<HttpResponseMessage> GetLoggedInUser(string token)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(AppStrings.ApiBaseUrl);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                return await client.GetAsync(AppStrings.GetUserEndpoint);
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

        public async Task<string> GetToken(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var loginResult = JsonSerializer.Deserialize<LoginResponse>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return loginResult?.Token!;
        }

        public async Task<UserRegisterDTO> GetUser(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserRegisterDTO>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return user!;
        }
    }
}
