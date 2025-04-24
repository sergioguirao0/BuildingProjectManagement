namespace BuildingProjectManagementAPI.Resources
{
    public class ApiStrings
    {
        public const string UserRoute = "api/users";
        public const string ContactRoute = "api/contacts";
        public const string ProjectRoute = "api/projects";

        public const string StringLengthMessage = "El campo {0} debe tener {1} caracteres o menos";
        public const string RequiredMessage = "El campo {0} es obligatorio";

        public const string ContactExist = "El contacto ya existe";
        public const string ContactCreationError = "Error al crear el contacto";
        public const string ContactCreated = "Contacto creado correctamente";
        public const string ContactUpdated = "Contacto actualizado correctamente";
        public const string ContactUpdateError = "Error al actualizar el contacto";
        public const string ContactDeleteError = "Error al borrar el contacto";
        public const string ContactDeleted = "Contacto eliminado correctamente";

        public const string ProjectContactExist = "Se ha seleccionado un contacto que no existe";
        public const string ProjectCreationError = "Error al crear el proyecto";
        public const string ProjectCreated = "Proyecto creado correctamente";
    }
}
