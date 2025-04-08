using BuildingProjectManagement.Model;
using BuildingProjectManagement.Resources.Strings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuildingProjectManagement.ViewModel
{
    public class UserViewModel : ViewModelBase
    {
        private string? _message;
        public string? Message
        {
            get => _message;
            set
            {
                if (_message != value)
                {
                    _message = value;
                    OnPropertyChanged(nameof(Message));
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
                    Message = AppStrings.RegisterSuccess;
                }
                else
                {
                    Message = AppStrings.RegisterFail;
                }
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        private string ChangeFirstChar(string text)
        {
            string resultado = "";

            if (!string.IsNullOrEmpty(text))
            {
                if (text.Contains(" "))
                {
                    string[] words = text.Split(" ");

                    for (int i = 0; i < words.Length; i++)
                    {
                        resultado += char.ToUpper(words[i][0]) + words[i].Substring(1);

                        if (i != words.Length - 1)
                        {
                            resultado += " ";
                        }
                    }
                }
                else
                {
                    resultado += char.ToUpper(text[0]) + text.Substring(1);
                }
            }

            return resultado;
        }

        private bool CheckDni(string dni)
        {
            return false;
        }
    }
}
