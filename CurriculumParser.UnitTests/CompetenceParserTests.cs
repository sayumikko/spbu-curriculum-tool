using System;
using NUnit.Framework;
using CurriculumParser;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.Linq;

namespace CurriculumParser.UnitTests;

public class Tests
{
    [Test]
    public void correctCompetenceTest()
    {
        var doc = getCorrectTable();
        var parsedTable = CurriculumParser.DocxCompetencesParser.Parse(doc.Body);

        var expectedTable = new List<Competence>();
        for (int i = 1; i <= 3; i++)
        {
            var competence = new Competence("ТК-" + i, "Описание компетенции " + i);
            expectedTable.Add(competence);
        }

        Assert.True(parsedTable.SequenceEqual(parsedTable));
    }

    private Document getCorrectTable()
    {
        using (WordprocessingDocument document = WordprocessingDocument.Create("docName.docx", WordprocessingDocumentType.Document))
        {
            document.AddMainDocumentPart();
            document.MainDocumentPart.Document = new Document(new Body());
            var doc = document.MainDocumentPart.Document;
            Table table = new Table();

            for (var i = 0; i <= 3; i++)
            {
                var tr = new TableRow();
                for (var j = 0; j < 2; j++)
                {
                    var tc = new TableCell();

                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            tc.Append(new Paragraph(new Run(new Text("Код компетенции"))));
                        }
                        else
                        {
                            tc.Append(new Paragraph(new Run(new Text("Наименование и (или) описание компетенции"))));
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            tc.Append(new Paragraph(new Run(new Text("ТК-" + i))));
                        }
                        else
                        {
                            tc.Append(new Paragraph(new Run(new Text("Описание компетенции " + i))));
                        }
                    }

                    tc.Append(new TableCellProperties(
                        new TableCellWidth { Type = TableWidthUnitValues.Auto }));

                    tr.Append(tc);
                }
                table.Append(tr);
            }

            doc.Body.Append(new Table());
            doc.Body.Append(new Table());
            doc.Body.Append(table);

            return doc;
        }
    }
}