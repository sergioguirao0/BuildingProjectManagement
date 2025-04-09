using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.Resources.Strings
{
    public class AppStrings
    {
        // Rutas
        public const string ApiBaseUrl = "https://localhost:7269/";
        public const string RegisterEndpoint = "api/usuarios/registro";

        public const string ApplicationJson = "application/json";

        // Mensajes de error o confirmación
        public const string RegisterSuccess = "Registro completado con éxito";
        public const string RegisterFail = "Registro fallido";
        public const string CheckName = "El campo Nombre/Apellidos no puede estar vacío";
        public const string CheckDni = "Formato DNI/NIE no válido";
    }
}
