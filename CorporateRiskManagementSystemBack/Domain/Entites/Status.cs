using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CorporateRiskManagementSystemBack.Domain.Entites
{
    public class Status
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StatusId { get; set; }

        public int RiskId { get; set; }

        [Required]
        [MaxLength(50)]
        public string StatusName { get; set; } = null!;

        public string? StatusDescription { get; set; }

        public DateTime ChangedAt { get; set; }

        public int ChangedById { get; set; }

        // Навигационные свойства
        public virtual Risk Risk { get; set; } = null!;
        public virtual User ChangedBy { get; set; } = null!;
    }
}
