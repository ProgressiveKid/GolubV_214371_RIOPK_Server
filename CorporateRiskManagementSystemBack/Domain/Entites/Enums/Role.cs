using System.Runtime.Serialization;

namespace CorporateRiskManagementSystemBack.Domain.Entites.Enums
{
    /// <summary>
    /// Перечисление для ролей пользователей
    /// </summary>
    public enum Role
    {
        /// <summary>
        /// Роль администратора
        /// </summary>
        [EnumMember(Value = "Administrator")]
        Administrator,

        /// <summary>
        /// Директор
        /// </summary>
        [EnumMember(Value = "Manager")]
        Manager,

        /// <summary>
        /// Аудитор
        /// </summary>
        [EnumMember(Value = "Auditor")]
        Auditor
    }
}
