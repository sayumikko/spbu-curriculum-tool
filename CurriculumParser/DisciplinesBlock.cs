using System.Collections.Generic;

namespace CurriculumParser
{
    /// <summary>
    /// Содержит информацию о блоке дисциплин
    /// </summary>
    public class DisciplinesBlock
    {
        /// <summary>
        /// Создает экзкмпляр класса <name>DisciplinesBlock</name>
        /// </summary>
        /// <param name="semester">Семестр, в котором доступны дисциплины блока</param>
        public DisciplinesBlock(string name, int semester)
        {
            Name = name;
            Semester = semester;
            Implementations = new List<DisciplineImplementation>();
        }

        /// <summary>
        /// Название блока дисциплин
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Семестр, в котором дисциплины блока доступны для выбора
        /// </summary>
        public int Semester { get; private set; }

        /// <summary>
        /// Реализация дисциплины, содержащаяся в данном блоке
        /// </summary>
        public List<DisciplineImplementation> Implementations { get; private set; }
    }
}
