using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CorporateRiskManagementSystemBack.Domain.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();

            return attribute?.GetDescription() ?? value.ToString();
        }

        public static string GetName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DisplayAttribute>();

            return attribute?.GetName() ?? value.ToString();
        }
    }
}
