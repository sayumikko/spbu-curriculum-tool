namespace CurriculumParser
{
    /// <summary>
    /// Описывает тип занятия, которому соответсвует число часов в таблице с организацией обучения. 
    /// Типы занятий могут быть организованы одним или несколькими способами, описанными в LessonOrganizationType 
    /// </summary>
    public enum LessonType
    {
        Lecture,
        Seminar,
        Consultation,
        Workshops,
        LaboratoryWorks,
        TestPapers,
        Colloquia,
        CurrentControl,
        IntermediateCertification,
        UnderTheGuidanceOfATeacher,
        InThePresenceOfATeacher,
        IncludingUsingEducationalAndMethodologicalMaterials,
        TheVolumeOfClassesInActiveAndInteractiveForms
    }
}
