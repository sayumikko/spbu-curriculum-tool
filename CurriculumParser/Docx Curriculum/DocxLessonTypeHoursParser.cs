using System.Collections.Generic;

namespace CurriculumParser
{
    /// <summary>
    /// Парсер типов работ
    /// </summary>
    class DocxLessonTypeHoursParser
    {
        private static readonly List<LessonType> orderedAsInTableLessonTypes = new List<LessonType>
        {
            LessonType.Lecture, LessonType.Seminar,
            LessonType.Consultation, LessonType.Workshops,
            LessonType.LaboratoryWorks, LessonType.TestPapers,
            LessonType.Colloquia, LessonType.CurrentControl,
            LessonType.IntermediateCertification, LessonType.UnderTheGuidanceOfATeacher,
            LessonType.InThePresenceOfATeacher,
            LessonType.IncludingUsingEducationalAndMethodologicalMaterials,
            LessonType.CurrentControl, LessonType.IntermediateCertification,
            LessonType.TheVolumeOfClassesInActiveAndInteractiveForms
        };

        public static List<LessonTypeHours> ParseLessonTypeHours(List<string> hours)
        {
            if (hours.Count != 15)
            {
                throw new CurriculumParsingException("В строке с распределением часов по типам занятий нестандартное число колонок");
            }

            var lessonTypeHours = new List<LessonTypeHours>();
            for (var i = 0; i < hours.Count; ++i)
            {
                var superiorType = i == 14 
                    ? LessonOrganizationType.NoType 
                    : i < 9 
                        ? LessonOrganizationType.ClassroomWork 
                        : LessonOrganizationType.IndependentWork;

                lessonTypeHours.Add(new LessonTypeHours(hours[i], orderedAsInTableLessonTypes[i], superiorType));
            }

            return lessonTypeHours;
        }
    }
}