using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurriculumParser
{
    public class Examination
    {
        /// <summary>
        /// Русское наименование итоговой аттестации
        /// </summary>
        public string RussianName { get; private set; }

        /// <summary>
        /// Английское наименование итоговой аттестации
        /// </summary>
        public string EnglishName { get; private set; }

        /// <summary>
        /// Трудоемкость в зачетных единицах
        /// </summary>
        public int LaborIntensity { get; private set; }

        /// <summary>
        /// Формируемые компетенции
        /// </summary>
        public List<Competence> Competences { get; private set; }

        public Examination(string russianName, string englishName, int laborIntensity, List<Competence> competences)
        {
            RussianName = russianName;
            EnglishName = englishName;
            LaborIntensity = laborIntensity;
            Competences = competences;
        }
    }
}