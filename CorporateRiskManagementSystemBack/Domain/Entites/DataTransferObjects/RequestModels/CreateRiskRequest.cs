namespace CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels
{
    public class CreateRiskRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public string Likelihood { get; set; }
        public int DepartmentId { get; set; }  // ID отдела
        public string UsernameId { get; set; }

    }
}
