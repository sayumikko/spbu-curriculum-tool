using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CurriculumParser
{
    public class DocxExaminationParser
    {
        private List<Competence> competences;
        private Body body;

        public DocxExaminationParser(Body body, List<Competence> competences)
        {
            this.body = body;
            this.competences = competences;
        }


        public List<Examination> Parse()
        {
            var prevIntensity = 0;
            List<Competence> prevCompetences = null;
            try
            {
                var examinations = new List<Examination>();
                var table = body.Elements<Table>().Skip(4).First();
                var rows = table.Elements<TableRow>().Skip(2);
                foreach (var row in rows)
                {
                    if (row.Descendants<TableCell>().Count() == 1) continue;

                    var nameCell = row.Elements<TableCell>().ElementAt(2);
                    var (rusName, engName) = ParseName(nameCell);

                    var competencesCell = row.Elements<TableCell>().ElementAt(3);
                    if (!string.IsNullOrEmpty(competencesCell?.InnerText))
                    {
                        prevCompetences = ParseDisciplineCompetences(competencesCell);
                    }

                    var intensityCell = row.Elements<TableCell>().ElementAt(1);
                    if (!string.IsNullOrEmpty(intensityCell?.InnerText))
                    {
                        prevIntensity = int.Parse(intensityCell.InnerText);
                    }

                    var examination = new Examination(rusName, engName, prevIntensity, prevCompetences);
                    examinations.Add(examination);
                }

                return examinations;
            }
            catch (Exception e)
            {
                throw new CurriculumParsingException("Ошибка парсинга выпускных работ .docx", e);
            }
        }

        private bool TryParseDisciplineType(string text, out DisciplineType type)
        {
            switch (text.Trim())
            {
                case "Базовая часть периода обучения":
                    type = DisciplineType.Base;
                    return true;
                case "Вариативная часть периода обучения":
                    type = DisciplineType.Elective;
                    return true;
            }
            type = default;
            return false;
        }

        private (string Name, string EngName) ParseName(TableCell cell)
        {
            var name = "";
            var engName = "";
            var seenBreak = false;
            var runs = cell.Descendants<Run>();
            foreach (var run in runs)
            {
                foreach (var element in run.ChildElements)
                {
                    if (element.LocalName == "br")
                    {
                        seenBreak = true;
                        continue;
                    }
                    if (seenBreak)
                    {
                        engName += element.InnerText;
                        continue;
                    }
                    name += element.InnerText;
                }
            }

            var regexMatch = Regex.Match(name, @"\[(\d+)\]\s+([^(]+)\(?([^)]+)?\)?,?\s?(.+)?");
            if (regexMatch.Success)
            {
                var rusName = regexMatch.Groups[2].Value.Trim();
                var match = Regex.Match(engName, @"[^(]+");
                if (match.Success)
                {
                    engName = match.Value.Trim();
                }
                return (rusName, engName);
            }
            return default;
        }

        private List<Competence> ParseDisciplineCompetences(TableCell cell)
        {
            var codes = cell.InnerText.Replace(" ", "").Split(',');
            return competences.Where(c => codes.Contains(c.Code)).ToList();
        }
    }
}