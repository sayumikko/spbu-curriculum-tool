using System;
using NUnit.Framework;
using CurriculumParser;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.Linq;

namespace CurriculumParser.UnitTests;

public class CompetenceTests
{
    [Test]
    public void correctCompetenceTest()
    {
        var examPars = getCorrectTable();
        var parsed = examPars.Parse();

        Assert.IsNotEmpty(parsed);
        Assert.True(parsed.Count == 1);
    }

    private DocxExaminationParser getCorrectTable()
    {
        using (WordprocessingDocument document = WordprocessingDocument.Create("docName.docx", WordprocessingDocumentType.Document))
        {
            document.AddMainDocumentPart();
            document.MainDocumentPart.Document = new Document(new Body());
            var doc = document.MainDocumentPart.Document;
            Table table = new Table();

            for (int i = 0; i < 4; i++)
            {
                table.Append(new TableRow(new TableCell(new Paragraph(new Run(new Text("Текст"))))));
            }

            var examRow = new TableRow();

            var run = new Run();
            run.Append(new Text("Защита выпускной квалификационной работы"));
            run.Append(new Break());
            run.Append(new Text("Qualification Research Paper Defense"));

            examRow.Append(new TableCell(new Paragraph(new Run(new Text("Блок 3")))));
            examRow.Append(new TableCell(new Paragraph(new Run(new Text("6")))));
            examRow.Append(new TableCell(new Paragraph(run)));
            examRow.Append(new TableCell(new Paragraph(new Run(new Text("ОПК-1, ОПК-2, ОПК-3")))));

            table.Append(examRow);

            doc.Body.Append(new Table());
            doc.Body.Append(new Table());
            doc.Body.Append(new Table());
            doc.Body.Append(new Table());
            doc.Body.Append(table);

            var comps = new List<Competence>();
            comps.Append(new Competence("ОПК-1", "Тестовая компетенция"));
            comps.Append(new Competence("ОПК-2", "Тестовая компетенция"));
            comps.Append(new Competence("ОПК-3", "Тестовая компетенция"));

            var examParser = new DocxExaminationParser(doc.Body, comps);

            return examParser;
        }
    }
}