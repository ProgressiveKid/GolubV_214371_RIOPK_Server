using System;
using System.Collections.Generic;

namespace CorporateRiskManagementSystemBack.Domain.Entites
{
    public partial class User
    {
        public User()
        {
            AuditReports = new HashSet<AuditReport>();
            RiskAssessments = new HashSet<RiskAssessment>();
            Risks = new HashSet<Risk>();
        }

        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Role { get; set; } = null!;

        public virtual ICollection<AuditReport> AuditReports { get; set; }
        public virtual ICollection<RiskAssessment> RiskAssessments { get; set; }
        public virtual ICollection<Risk> Risks { get; set; }
    }
}
