﻿using DocumentFormat.OpenXml.Packaging;
using System.Collections.Generic;

namespace CurriculumParser
{
    /// <summary>
    /// Содержит информацию об учебном плане с расширением .docx
    /// </summary>
    public class DocxCurriculum : ICurriculumWithElectiveBlocks
    {
        /// <summary>
        /// Код учебного плана
        /// </summary>
        public string CurriculumCode { get; private set; }

        /// <summary>
        /// Программа
        /// </summary>
        public Programme Programme { get; private set; }

        /// <summary>
        /// Дисциплины
        /// </summary>
        public List<Discipline> Disciplines { get; private set; }

        /// <summary>
        /// Элективные блоки
        /// </summary>
        public List<ElectivesBlock> ElectiveBlocks { get; private set; }

        /// <summary>
        /// Блоки дисциплин
        /// </summary>
        public List<DisciplinesBlock> DisciplinesBlocks { get; private set; }

        /// <summary>
        /// Компетенции
        /// </summary>
        public List<Competence> Competences { get; private set; }

        /// <summary>
        /// Выпускная квалификационная работа
        /// </summary>
        public List<Examination> Examinations { get; private set; }

        /// <summary>
        /// Создает экземляр класса <name>DocxCurriculum</name>
        /// </summary>
        /// <param name="fileName">Имя файла с расширением .docx в котором содержится </param>
        public DocxCurriculum(string fileName)
        {
            var wordDocument = WordprocessingDocument.Open(fileName, false);
            var body = wordDocument.MainDocumentPart.Document.Body;

            CurriculumCode = DocxCurriculumCodeParser.Parse(body);
            Programme = DocxProgrammeParser.Parse(body);
            Competences = DocxCompetencesParser.Parse(body);
            ElectiveBlocks = new List<ElectivesBlock>();
            DisciplinesBlocks = new List<DisciplinesBlock>();

            var disciplinesParser = new DocxDisciplinesParser(body, Competences, Programme, ElectiveBlocks, DisciplinesBlocks);
            Disciplines = disciplinesParser.Parse();

            var examinationsParser = new DocxExaminationParser(body, Competences);
            Examinations = examinationsParser.Parse();
        }
    }
}