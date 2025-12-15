using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using CorporateRiskManagementSystemBack.Domain.Entites.Enums;

namespace CorporateRiskManagementSystemBack.Domain.Entites
{
    public partial class Risk
    {
        public Risk()
        {
            RiskAssessments = new HashSet<RiskAssessment>();
            Departments = new HashSet<Department>();
            Statuses = new HashSet<Status>();
        }

        public int RiskId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string Severity { get; set; } = null!;
        public string Likelihood { get; set; } = null!;
        public RiskType RiskType { get; set; }
        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }

        public virtual User CreatedBy { get; set; } = null!;
        public virtual ICollection<RiskAssessment> RiskAssessments { get; set; }
        public virtual ICollection<Department> Departments { get; set; }
        public virtual ICollection<Status> Statuses { get; set; }

        [NotMapped]
        public bool IsHaveAssessment => RiskAssessments.Count != 0;

        [NotMapped]
        public Status? CurrentStatus => Statuses?.OrderByDescending(s => s.ChangedAt).FirstOrDefault(); // Дополнительное свойство для удобства
    }
}
