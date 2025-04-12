namespace BuildingProjectManagementAPI.Model.DTO
{
    public class AuthenticationResponseDTO
    {
        public required string Token { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
