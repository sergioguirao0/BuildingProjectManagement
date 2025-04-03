using BuildingProjectManagement.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuildingProjectManagement.ViewModel
{
    public class UserViewModel
    {
        public async Task Register(User user)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7269/");

                var json = JsonSerializer.Serialize(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/usuarios/registro", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Registro completado con éxito");
                }
                else
                {
                    Console.WriteLine("Registro fallido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void ChangeFirstChar(string text)
        {
            if (text.Contains(" "))
            {
                string[] words = text.Split(" ");

                foreach (string word in words)
                {
                    Console.WriteLine(word);
                }
            }
        }
    }
}
