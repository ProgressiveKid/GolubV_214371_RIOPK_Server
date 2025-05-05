using System;
using System.Collections.Generic;

namespace CorporateRiskManagementSystemBack.Domain.Entites
{
    public partial class AuditReport
    {
        public int ReportId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int AuthorId { get; set; }
        public string? Content { get; set; }

        public int DepartmentId { get; set; }

        public virtual User Author { get; set; } = null!;

        public virtual Department Department { get; set; } = null!;

    }
}
