using System.Collections.Generic;
using System.Linq;

namespace CurriculumParser
{
    public class Semester
    {
        public int Number { get; private set; }

        public List<DisciplineImplementation> Implementations = new List<DisciplineImplementation>();

        public int LaborIntensity { get; private set; }

        public Semester(int number, DocxCurriculum docxCurriculum)
        {
            Number = number;
            Implementations = ParseImplementations(docxCurriculum);
            LaborIntensity = CountLaborIntensity();
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

        private int CountLaborIntensity()
        {
            int count = 0;
            List<DisciplineImplementation> disciplines = new List<DisciplineImplementation>();
            var codes = new List<string>();
            var blocks = new List<int>();
            foreach (DisciplineImplementation imp in Implementations)
            {
                Discipline discipline = imp.Discipline;
                if (!codes.Contains(discipline.Code))
                {
                    if (discipline.Type is DisciplineType.Base)
                    {
                        codes.Add(discipline.Code);
                        count += imp.LaborIntensity;
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
            return count;
        }
    }
}