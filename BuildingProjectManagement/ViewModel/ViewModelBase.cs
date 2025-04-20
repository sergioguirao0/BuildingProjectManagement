using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BuildingProjectManagement.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

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

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(AppStrings.ApiBaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ActualSession.Session.Token);
            return client;
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

        public bool CheckName(string name)
        {
            return !string.IsNullOrEmpty(name);
        }

        public bool CheckIdentification(string identification)
        {
            if (identification.Length != 9)
            {
                return false;
            }
            else
            {
                string regexDni = "^\\d{8}[A-Z]$";
                string regexNie = "^[XYZ]\\d{7}[A-Z]$";
                string regexCif = "^[A-Z]\\d{7}[0-9A-J]$";

                return Regex.IsMatch(identification, regexDni) || Regex.IsMatch(identification, regexNie) 
                    || Regex.IsMatch(identification, regexCif);
            }
        }

        public bool CheckEmail(string email)
        {
            string regexEmail = "^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, regexEmail);
        }
    }
}
