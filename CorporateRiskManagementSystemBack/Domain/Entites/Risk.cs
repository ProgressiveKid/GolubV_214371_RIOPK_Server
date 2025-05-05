using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateRiskManagementSystemBack.Domain.Entites
{
    public partial class Risk
    {
        public Risk()
        {
            RiskAssessments = new HashSet<RiskAssessment>();
            Departments = new HashSet<Department>();
        }

        public int RiskId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Severity { get; set; } = null!;
        public string Likelihood { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }

        public virtual User CreatedBy { get; set; } = null!;
        public virtual ICollection<RiskAssessment> RiskAssessments { get; set; }

        public virtual ICollection<Department> Departments { get; set; }

        [NotMapped]
        public bool IsHaveAssessment => RiskAssessments.Count != 0;
    }
}
