using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingProjectManagement.Resources.Strings
{
    public class AppStrings
    {
        public const string ApiBaseUrl = "https://localhost:7269/";
        
        // Rutas
        public const string RegisterEndpoint = "api/users/register";
        public const string LoginEndpoint = "api/users/login";
        public const string GetUserEndpoint = "api/users/loggedInUser";
        public const string ProjectEndpoint = "api/projects";
        public const string ContactsEndpoint = "api/contacts";
        public const string ApplicationJson = "application/json";
        public const string ApplicationPdf = "application/pdf";
        public const string DocumentEndpoint = "/documents";

        // Mensajes de error o confirmación
        // Mensajes de usuario e inicio de sesión
        public const string RegisterSuccess = "Registro completado con éxito";
        public const string RegisterFail = "Registro fallido: Usuario ya registrado en la base de datos";
        public const string ConfirmationMessage = "Confirmación";
        public const string ErrorMessage = "Error";
        public const string LoginError = "Correo electrónico o contraseña incorrectos";

        // Mensajes de comprobación formularios
        public const string CheckName = "El campo Nombre no puede estar vacío";
        public const string CheckSurname = "El campo Apellidos no puede estar vacío";
        public const string CheckDni = "DNI/NIE no válido";
        public const string CheckDniContact = "DNI/CIF no válido";
        public const string CheckEmail = "Email no válido";
        public const string CheckPhone = "Número de teléfono no válido";
        public const string CheckProfession = "El campo Profesión no puede estar vacío";
        public const string CheckPasswordFormat = "La contraseña debe incluir un número, una letra minúscula, una letra mayúscula y un símbolo";
        public const string CheckPassword = "Los dos campos contraseña no coinciden";
        public const string CheckSite = "El campo Emplazamiento no puede estar vacío";
        public const string CheckJobType = "El campo Tipo de trabajo no puede estar vacío";
        public const string CheckState = "Se debe seleccionar un estado para el proyecto";
        
        // Mensajes de interacción con la tabla contactos
        public const string ContactCreateError = "Error al crear el contacto";
        public const string ContactUpdateError = "Error al actualizar el contacto";
        public const string ContactDeleteError = "Error al borrar el contacto";
        public const string ContactDeleteErrorSelection = "No hay ningún contacto seleccionado";
        public const string ConfirmationWindowUpdateTitle = "Actualizar contacto";
        public const string ConfirmationWindowDeleteTitle = "Eliminar contacto";
        public const string ConfirmationWindowUpdateMessage = "Se va a actualizar el contacto: ";
        public const string ConfirmationWindowDeleteMessage = "Se va a eliminar el contacto: ";

        // Items strings de elementos
        public static string[] StateItems = { "No iniciado", "En ejecución", "Finalizado", "Cancelado" };
        public const string StateAllProjects = "Todos los proyectos";
        public const string ProjectCategory = "Proyecto";
        public const string StartingProjectCategory = "Documentos previos al inicio del proyecto";
        public const string ExecutionProjectCategory = "Documentos relativos a la ejecución del proyecto";
        public const string EndingProjectCategory = "Documentos relativos al final del proyecto";
        public const string OtherDocumentsCategory = "Otros documentos";
        public static string[] CategoryItems = {ProjectCategory, StartingProjectCategory, ExecutionProjectCategory,
            EndingProjectCategory, OtherDocumentsCategory};
        public const string PdfFilter = "Archivos PDF (*.pdf)|*.pdf";

        // Mensajes formulario proyectos
        public const string NoSelectedContactError = "No se ha seleccionado ningún contacto";
        public const string ContactInListError = "El contacto seleccionado ya está incluido en la lista";
        public const string EmptyListError = "La lista de contactos está vacía";
        public const string ProjectCreationError = "Error al crear el proyecto";
        public const string ProjectDeleteTitle = "Borrar proyecto";
        public const string ProjectUpdateTitle = "Actualizar proyecto";
        public const string ProjectDeleteMessage = "Se va a borrar el proyecto seleccionado junto con todos sus documentos";
        public const string ProjectUpdateMessage = "Se va a actualizar el proyecto seleccionado";
        public const string NoSelectedProject = "No hay ningún proyecto de la lista seleccionado";
        public const string DeleteProjectError = "Error al borrar el proyecto";
        public const string UpdateProjectError = "Error al actualizar el proyecto";

        // Mensajes formulario documentos
        public const string NoTitleError = "El campo Título no puede estar vacío";
        public const string NoCategoryError = "El campo Categoría no puede estar vacío";
        public const string NoFilePathError = "No se ha seleccionado ningún documento";
        public const string UploadDocumentError = "Error al guardar el documento";
        public const string DeleteDocumentError = "Error al borrar el documento";
        public const string NoSelectedDocumentError = "No hay ningún documento seleccionado";
    }
}
