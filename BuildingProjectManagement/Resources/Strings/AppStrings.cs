﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.Resources.Strings
{
    public class AppStrings
    {
        public const string ApiBaseUrl = "https://localhost:7269/";
        
        // Rutas usuario
        public const string RegisterEndpoint = "api/users/register";
        public const string LoginEndpoint = "api/users/login";
        public const string GetUserEndpoint = "api/users/loggedInUser";

        // Rutas contacto
        public const string ContactsEndpoint = "api/contacts";

        public const string ApplicationJson = "application/json";

        // Mensajes de error o confirmación
        public const string RegisterSuccess = "Registro completado con éxito";
        public const string RegisterFail = "Registro fallido: Usuario ya registrado en la base de datos";
        public const string ConfirmationMessage = "Confirmación";
        public const string ErrorMessage = "Error";
        public const string CheckName = "El campo Nombre/Apellidos no puede estar vacío";
        public const string CheckDni = "DNI/NIE no válido";
        public const string CheckEmail = "Email no válido";
        public const string CheckPasswordFormat = "La contraseña debe incluir un número, una letra minúscula, una letra mayúscula y un símbolo";
        public const string CheckPassword = "Los dos campos contraseña no coinciden";
        public const string LoginError = "Correo electrónico o contraseña incorrectos";
    }
}
