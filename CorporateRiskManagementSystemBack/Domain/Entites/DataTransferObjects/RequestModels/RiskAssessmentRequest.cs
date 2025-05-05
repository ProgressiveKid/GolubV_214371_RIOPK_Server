namespace CorporateRiskManagementSystemBack.Domain.Entites.DataTransferObjects.RequestModels
{
    public class RiskAssessmentRequest
    {
        public int RiskId { get; set; }
        public string UsernameId { get; set; }
        public DateTime AssessmentDate { get; set; }
        public int ImpactScore { get; set; }
        public int ProbabilityScore { get; set; }
        public string? Notes { get; set; }
    }
}
