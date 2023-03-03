namespace CurriculumParser
{
    /// <summary>
    /// Описывает тип занятия, количество выделенных для проведения часов и тип организации занятия
    /// </summary>
    public class LessonTypeHours
    {
        public LessonTypeHours(string hours, LessonType lessonType, 
            LessonOrganizationType lessonOrganizationType)
        {
            Hours = hours;
            Type = lessonType;
            OrganizationType = lessonOrganizationType;
        }

        /// <summary>
        /// Количество выделенных для данного типа занятия часов
        /// </summary>
        public string Hours { get; private set; }

        /// <summary>
        /// Тип занятия (лекция, коллоквиум, текущий контроль...)
        /// </summary>
        public LessonType Type { get; private set; }

        /// <summary>
        /// Тип организации занятия (аудиторная, самостоятельная работа...)
        /// </summary>
        public LessonOrganizationType OrganizationType { get; private set; }
    }
}
