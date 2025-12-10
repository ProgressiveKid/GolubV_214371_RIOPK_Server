using System.ComponentModel.DataAnnotations;
using System.Reflection;
using CorporateRiskManagementSystemBack.Domain.Entites.Enums;

namespace CorporateRiskManagementSystemBack.Domain.Common.Extensions
{
    public static class EnumExtensions
    {
        private static readonly Dictionary<RiskType, string> RussianNames = new Dictionary<RiskType, string>
        {
            { RiskType.Operational, "Операционный" },
            { RiskType.Financial, "Финансовый" },
            { RiskType.Legal, "Юридический" },
            { RiskType.Reputational, "Репутационный" },
            { RiskType.HumanResources, "Кадровый" },
            { RiskType.Technological, "Технологический" }
        };

        public static string GetName(this RiskType riskType)
        {
            return RussianNames.TryGetValue(riskType, out var name)
                ? name
                : riskType.ToString();
        }

        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();

            return attribute?.GetDescription() ?? value.ToString();
        }
    }
}
