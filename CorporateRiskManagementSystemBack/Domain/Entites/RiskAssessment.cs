using System;
using System.Collections.Generic;

namespace CorporateRiskManagementSystemBack.Domain.Entites
{
    public partial class RiskAssessment
    {
        public int AssessmentId { get; set; }
        public int RiskId { get; set; }
        public int AssessedById { get; set; }
        public DateTime AssessmentDate { get; set; }
        public short? ImpactScore { get; set; }
        public short? ProbabilityScore { get; set; }
        public string? Notes { get; set; }

        public virtual User AssessedBy { get; set; } = null!;
        public virtual Risk Risk { get; set; } = null!;

    }
}
