using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
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
                    OnPropertyChanged(nameof(RegisterMessage));
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
                    RegisterMessage = AppStrings.RegisterSuccess;
                }
                else
                {
                    RegisterMessage = AppStrings.RegisterFail;
                }
            }
            catch (Exception ex)
            {
                RegisterMessage = ex.Message;
            }
        }

        private string ChangeFirstChar(string text)
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
            return false;
        }
    }
}
