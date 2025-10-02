using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CorporateRiskManagementSystemBack.Domain.Entites.Enums
{
    /// <summary>
    /// Перечисление для типов корпоративных рисков
    /// </summary>
    public enum RiskType
    {
        /// <summary>
        /// Операционные риски
        /// </summary>
        [EnumMember(Value = "Operational")]
        [Display(Description = "Сбои в работе оборудования, проблемы с поставками, ошибки в управлении запасами, недостаточная квалификация персонала")]
        Operational,

        /// <summary>
        /// Финансовые риски
        /// </summary>
        [EnumMember(Value = "Financial")]
        [Display(Description = "Проблемы с ликвидностью, нехватка финансирования, ошибки в управлении кредитным портфелем")]
        Financial,

        /// <summary>
        /// Юридические риски
        /// </summary>
        [EnumMember(Value = "Legal")]
        [Display(Description = "Судебные иски, нарушения законодательства, нарушение авторских прав, несоблюдение налоговых обязательств")]
        Legal,

        /// <summary>
        /// Репутационные риски
        /// </summary>
        [EnumMember(Value = "Reputational")]
        [Display(Description = "Ущерб деловой репутации из-за действий сотрудников или внешних событий")]
        Reputational,

        /// <summary>
        /// Кадровые риски
        /// </summary>
        [EnumMember(Value = "HumanResources")]
        [Display(Description = "Некомпетентность сотрудников, недобросовестность, мошенничество")]
        HumanResources,

        /// <summary>
        /// Технологические риски
        /// </summary>
        [EnumMember(Value = "Technological")]
        [Display(Description = "Технические сбои, устаревание технологий, проблемы с безопасностью данных")]
        Technological
    }
}
