﻿namespace BuildingProjectManagementAPI.Resources
{
    public class ApiStrings
    {
        // Routes
        public const string AzureConnectionString = "AzureStorageConnection";
        public const string UserRoute = "api/users";
        public const string ContactRoute = "api/contacts";
        public const string ProjectRoute = "api/projects";
        public const string DocumentRoute = "/documents";
        public const string AllowedHosts = "buildingprojectmanagementapi20250528194053-hra9b8gjewgra9h6.westeurope-01.azurewebsites.net;localhost";

        // Control messages
        public const string StringLengthMessage = "El campo {0} debe tener {1} caracteres o menos";
        public const string RequiredMessage = "El campo {0} es obligatorio";

        // Contact messages
        public const string ContactExist = "El contacto ya existe";
        public const string ContactCreationError = "Error al crear el contacto";
        public const string ContactCreated = "Contacto creado correctamente";
        public const string ContactUpdated = "Contacto actualizado correctamente";
        public const string ContactUpdateError = "Error al actualizar el contacto";
        public const string ContactDeleteError = "Error al borrar el contacto";
        public const string ContactDeleted = "Contacto eliminado correctamente";

        // Project messages
        public const string ProjectContactExist = "Se ha seleccionado un contacto que no existe";
        public const string ProjectCreationError = "Error al crear el proyecto";
        public const string ProjectCreated = "Proyecto creado correctamente";
        public const string ProjectNotFound = "Proyecto no encontrado";
        public const string ProjectDeleted = "Proyecto eliminado correctamente";
        public const string ProjectDeleteError = "Error al borrar el proyecto";

        // Document messages
        public const string DocumentContainer = "documents";
        public const string DocumentCreationError = "Error al guardar el documento";
        public const string DocumentCreated = "Documento guardado correctamente";
        public const string DocumentNotFound = "No se ha encontrado el documento";
        public const string DocumentDeleted = "Documento borrado correctamente";
        public const string DocumentDeleteError = "Error al borrar el documento";
    }
}
