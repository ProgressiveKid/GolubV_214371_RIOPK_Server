using System;
using System.Collections.Generic;

namespace CorporateRiskManagementSystemBack.Domain.Entites
{
    public partial class Department
    {
        public Department()
        {
            Risks = new HashSet<Risk>();
        }

        public int DepartmentId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual ICollection<Risk> Risks { get; set; }

        public virtual ICollection<AuditReport> AuditReports { get; set; } = new List<AuditReport>();
    }
}
