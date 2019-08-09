using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Text.RegularExpressions;
// using Microsoft.Office.Interop.Word;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;

namespace translateLib
{
    public class wordHelper
    {
        public static void WordTranslate(string fromLang, string toLang)
        {
            var n = DateTime.Now;
            var fileIdnetity = string.Format("{0}{1}{2}{3}{4}{5}", n.Year, n.Month, n.Day, n.Hour, n.Minute, n.Second);
            var tempDi = new DirectoryInfo(string.Format("../translateDoc/{0}_{1}", "en", fileIdnetity));
            var tempDi2 = new DirectoryInfo(string.Format("../translateDoc/{0}_{1}", toLang, fileIdnetity));
            tempDi.Create();
            tempDi2.Create();
            //var fileName = "../translateDoc/cn/test.docx";
            var fileName = "";
            var fileNameFinal = "";
            var replaceText = "";
            var replaceTextFinal = "";
            List<Table> tables = new List<Table>();
            List<TableRow> rows = new List<TableRow>();
            List<TableCell> cells = new List<TableCell>();
            DirectoryInfo di2 = new DirectoryInfo(string.Format("../translateDoc/{0}/", fromLang));

            // foreach (var file in di2.GetFiles("*.docx"))
            //     file.CopyTo(Path.Combine(tempDi.FullName, file.Name));

            // using (WordprocessingDocument doc = WordprocessingDocument.Open(Path.Combine(tempDi.FullName, "test.docx"), true))
            //     TextReplacer.SearchAndReplace(doc, "the", "***", false);

            foreach (var file in di2.GetFiles("*.docx"))
            {
                fileName = Path.Combine(tempDi.FullName, file.Name);
                fileNameFinal = Path.Combine(tempDi2.FullName, file.Name);
                file.CopyTo(fileName);
                file.CopyTo(fileNameFinal);
                using (WordprocessingDocument wdDoc = WordprocessingDocument.Open(file.FullName, true))
                {
                    Body body = wdDoc.MainDocumentPart.Document.Body;
                    List<OpenXmlElement> elements = body.Cast<OpenXmlElement>().ToList();

                    foreach (OpenXmlElement element in elements)
                    {
                        if (!string.IsNullOrEmpty(element.InnerText))
                        {
                            // replaceText = TranslationHelper.GetTranslation("en", element.InnerText);
                            replaceText = element.InnerText + "***";

                            // converted to english
                            using (WordprocessingDocument doc = WordprocessingDocument.Open(fileName, true))
                            {
                                OpenXmlPowerTools.TextReplacer.SearchAndReplace(doc, element.InnerText, replaceText, false);
                            }

                            // replaceTextFinal = TranslationHelper.GetTranslation(toLang, replaceText);
                            replaceTextFinal = element.InnerText + "***";
                            // convert from English to target language
                            using (WordprocessingDocument docFinal = WordprocessingDocument.Open(fileNameFinal, true))
                            {
                                OpenXmlPowerTools.TextReplacer.SearchAndReplace(docFinal, element.InnerText, replaceTextFinal, false);
                            }

                        }
                    }


                    // 加入表格的翻译
                    tables = wdDoc.MainDocumentPart.Document.Body.Elements<Table>().ToList();
                    if (tables != null)
                    {
                        foreach (Table table in tables)
                        {
                            if (table != null)
                            {
                                // Find the second row in the table.
                                rows = table.Elements<TableRow>().ToList();

                                foreach (TableRow row in rows)
                                {
                                    // Find the third cell in the row.
                                    cells = row.Elements<TableCell>().ToList();
                                    foreach (TableCell cell in cells)
                                    {
                                        if (!string.IsNullOrEmpty(cell.InnerText))
                                        {
                                            replaceText = TranslationHelper.GetTranslation("en", cell.InnerText);
                                            // replaceText = cell.InnerText + "***";
                                            // converted to english
                                            using (WordprocessingDocument doc = WordprocessingDocument.Open(fileName, true))
                                            {
                                                OpenXmlPowerTools.TextReplacer.SearchAndReplace(doc, cell.InnerText, replaceText, false);
                                            }

                                            replaceTextFinal = TranslationHelper.GetTranslation(toLang, replaceText);
                                            // replaceTextFinal = cell.InnerText + "***";
                                            // convert from English to target language
                                            using (WordprocessingDocument docFinal = WordprocessingDocument.Open(fileNameFinal, true))
                                            {
                                                OpenXmlPowerTools.TextReplacer.SearchAndReplace(docFinal, cell.InnerText, replaceTextFinal, false);
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        public static void WordTranslateForTable(string fromLang, string toLang)
        {
            var n = DateTime.Now;
            var fileIdnetity = string.Format("{0}{1}{2}{3}{4}{5}", n.Year, n.Month, n.Day, n.Hour, n.Minute, n.Second);
            var tempDi = new DirectoryInfo(string.Format("../translateDoc/{0}_{1}", "en", fileIdnetity));
            var tempDi2 = new DirectoryInfo(string.Format("../translateDoc/{0}_{1}", toLang, fileIdnetity));
            tempDi.Create();
            tempDi2.Create();
            //var fileName = "../translateDoc/cn/test.docx";
            var fileName = "";
            var fileNameFinal = "";
            var replaceText = "";
            var replaceTextFinal = "";
            List<Table> tables = new List<Table>();
            List<TableRow> rows = new List<TableRow>();
            List<TableCell> cells = new List<TableCell>();
            DirectoryInfo di2 = new DirectoryInfo(string.Format("../translateDoc/{0}/", fromLang));

            // foreach (var file in di2.GetFiles("*.docx"))
            //     file.CopyTo(Path.Combine(tempDi.FullName, file.Name));

            // using (WordprocessingDocument doc = WordprocessingDocument.Open(Path.Combine(tempDi.FullName, "test.docx"), true))
            //     TextReplacer.SearchAndReplace(doc, "the", "***", false);

            foreach (var file in di2.GetFiles("*.docx"))
            {
                fileName = Path.Combine(tempDi.FullName, file.Name);
                fileNameFinal = Path.Combine(tempDi2.FullName, file.Name);
                file.CopyTo(fileName);
                file.CopyTo(fileNameFinal);
                using (WordprocessingDocument wdDoc = WordprocessingDocument.Open(file.FullName, true))
                {
                    tables = wdDoc.MainDocumentPart.Document.Body.Elements<Table>().ToList();
                    if (tables != null)
                    {
                        foreach (Table table in tables)
                        {
                            if (table != null)
                            {
                                // Find the second row in the table.
                                rows = table.Elements<TableRow>().ToList();

                                foreach (TableRow row in rows)
                                {
                                    // Find the third cell in the row.
                                    cells = row.Elements<TableCell>().ToList();
                                    foreach (TableCell cell in cells)
                                    {
                                        if (!string.IsNullOrEmpty(cell.InnerText))
                                        {
                                            // replaceText = TranslationHelper.GetTranslation("en", cell.InnerText);
                                            replaceText = cell.InnerText + "***";

                                            // converted to english
                                            using (WordprocessingDocument doc = WordprocessingDocument.Open(fileName, true))
                                            {
                                                OpenXmlPowerTools.TextReplacer.SearchAndReplace(doc, cell.InnerText, replaceText, false);
                                            }

                                            // replaceTextFinal = TranslationHelper.GetTranslation(toLang, replaceText);
                                            replaceTextFinal = cell.InnerText + "***";
                                            // convert from English to target language
                                            using (WordprocessingDocument docFinal = WordprocessingDocument.Open(fileNameFinal, true))
                                            {
                                                OpenXmlPowerTools.TextReplacer.SearchAndReplace(docFinal, cell.InnerText, replaceTextFinal, false);
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }

                    // // Find the first paragraph in the table cell.
                    // Paragraph p = cell.Elements<Paragraph>().First();

                    // // Find the first run in the paragraph.
                    // Run r = p.Elements<Run>().First();

                    // // Set the text for the run.
                    // Text t = r.Elements<Text>().First();

                }
            }
        }
    }
}