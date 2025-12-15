namespace CorporateRiskManagementSystemBack.Domain.Entites.Enums
{
    public static class RiskStatuses
    {
        public const string NoAssessment = "Оценка отсутствует";
        public const string AssessmentCompleted = "Оценка произведена";
        public const string Completed = "Завершен";
        public const string Cancelled = "Отменен";

        // Получить все статусы
        public static List<string> GetAll()
        {
            return new List<string>
            {
                NoAssessment,
                AssessmentCompleted,
                Completed,
                Cancelled
            };
        }

        // Проверить, является ли статус допустимым
        public static bool IsValid(string status)
        {
            return GetAll().Contains(status);
        }
    }
}
