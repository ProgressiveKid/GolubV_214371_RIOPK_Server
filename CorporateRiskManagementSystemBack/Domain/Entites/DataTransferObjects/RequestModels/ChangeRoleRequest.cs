namespace CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels
{
    public class ChangeRoleRequest
    {
        public int UserId { get; set; }
        public string NewRole { get; set; } = "";
    }
}
