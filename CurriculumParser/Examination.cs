using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurriculumParser
{
    public class Examination
    {
        public string RussianName { get; private set; }
        public string EnglishName { get; private set; }
        public int LaborIntensity { get; private set; }
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