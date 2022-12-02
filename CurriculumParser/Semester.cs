using System.Collections.Generic;
using System.Linq;
using System;

namespace CurriculumParser
{
    public class Semester
    {
        /// <summary>
        /// Номер семестра
        /// </summary>
        public int Number { get; private set; }

        /// <summary>
        /// Список дисциплин, реализуемых в данном семестре
        /// </summary>
        public List<DisciplineImplementation> Implementations = new List<DisciplineImplementation>();

        /// <summary>
        /// Трудоемкость всего семестра в зачетных единицах
        /// </summary>
        public int LaborIntensity { get; private set; }

        public Semester(int number, DocxCurriculum docxCurriculum)
        {
            Number = number;
            Implementations = ParseImplementations(docxCurriculum);
            LaborIntensity = CountLaborIntensity(docxCurriculum);
        }

        private List<DisciplineImplementation> ParseImplementations(DocxCurriculum docxCurriculum)
        {
            List<DisciplineImplementation> implementations = new List<DisciplineImplementation>();
            List<Discipline> allDisciplines = docxCurriculum.Disciplines;
            foreach (Discipline disc in allDisciplines)
            {
                foreach (DisciplineImplementation imp in disc.Implementations)
                {
                    var semester = imp.Semester;
                    if (imp.Semester == Number)
                        implementations.Add(imp);
                }
            }
            return implementations;
        }

        private int CountLaborIntensity(DocxCurriculum docxCurriculum)
        {
            int count = 0;
            List<DisciplineImplementation> disciplines = new List<DisciplineImplementation>();
            List<DisciplinesBlock> disciplinesBlocks = new List<DisciplinesBlock>();
            foreach (DisciplinesBlock block in docxCurriculum.DisciplinesBlocks)
            {
                if (block.Semester == Number)
                {
                    disciplinesBlocks.Add(block);
                }
            }

            var codes = new List<string>();
            var blocks = new List<int>();
            foreach (DisciplineImplementation imp in Implementations)
            {
                Discipline discipline = imp.Discipline;
                if (!codes.Contains(discipline.Code))
                {
                    if (discipline.Type is DisciplineType.Base)
                    {
                        foreach (DisciplinesBlock block in disciplinesBlocks)
                        {
                            if (!block.Implementations.Contains(imp))
                            {
                                codes.Add(discipline.Code);
                                count += imp.LaborIntensity;
                            }
                        }
                        if (!disciplinesBlocks.Any())
                        {
                            codes.Add(discipline.Code);
                            count += imp.LaborIntensity;
                        }
                    }
                    if (discipline.ElectivesBlocks.Count > 0)
                    {
                        foreach (var block in discipline.ElectivesBlocks.Where(b =>
                                 b.Semester == Number))
                        {
                            if (!blocks.Contains(block.Number))
                            {
                                blocks.Add(block.Number);
                                count += imp.LaborIntensity;
                                foreach (var disc in block.Disciplines.Where(d =>
                                     d.Implementation.Semester == Number))
                                    codes.Add(disc.Discipline.Code);
                            }
                        }
                    }
                }
            }

            foreach (DisciplinesBlock block in disciplinesBlocks)
            {
                count += block.Implementations[0].LaborIntensity;
            }

            return count;
        }
    }
}