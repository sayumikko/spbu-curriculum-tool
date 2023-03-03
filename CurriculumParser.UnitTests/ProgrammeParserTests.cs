using System;
using NUnit.Framework;
using CurriculumParser;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.Linq;

namespace CurriculumParser.UnitTests;

public class ProgrammeTests
{
    [Test]
    public void correctProgrammeTest()
    {
        var doc = getCorrectTable();
        var parsedTable = CurriculumParser.DocxProgrammeParser.Parse(doc.Body);

        Assert.IsEmpty(parsedTable.Specializations);
        Assert.True(parsedTable.Code == "09.03.04");
        Assert.True(parsedTable.LevelOfEducation == "Бакалавриат");
        Assert.True(parsedTable.RussianName == "Программная инженерия");
        Assert.True(parsedTable.EnglishName == "Software engeneering");
    }

    private Document getCorrectTable()
    {
        using (WordprocessingDocument document = WordprocessingDocument.Create("docName.docx", WordprocessingDocumentType.Document))
        {
            document.AddMainDocumentPart();
            document.MainDocumentPart.Document = new Document(new Body());
            var doc = document.MainDocumentPart.Document;

            var run = new Run();
            run.Append(new Text("Программная инженерия"));
            run.Append(new Break());
            run.Append(new Text("Software engeneering"));

            doc.Body.Append(new Paragraph(run));

            Table table = new Table();

            for (var i = 0; i < 2; i++)
            {
                var tr = new TableRow();
                for (var j = 0; j < 2; j++)
                {
                    var tc = new TableCell();

                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            tc.Append(new Paragraph(new Run(new Text("По уровню"))));
                        }
                        else
                        {
                            tc.Append(new Paragraph(new Run(new Text("Бакалавриат"))));
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            tc.Append(new Paragraph(new Run(new Text("по направлению"))));
                        }
                        else
                        {
                            tc.Append(new Paragraph(new Run(new Text("09.03.04 Программная инженерия"))));
                        }
                    }

                    tc.Append(new TableCellProperties(
                        new TableCellWidth { Type = TableWidthUnitValues.Auto }));

                    tr.Append(tc);
                }
                table.Append(tr);
            }

            doc.Body.Append(table);

            return doc;
        }
    }
}