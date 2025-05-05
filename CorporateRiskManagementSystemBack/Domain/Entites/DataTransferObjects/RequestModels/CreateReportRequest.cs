namespace CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels
{
    public class CreateReportRequest
    {
        public int DepartmentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}
