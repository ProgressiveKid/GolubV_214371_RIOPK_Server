namespace CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels
{
    public class UserUpdateRequest
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
