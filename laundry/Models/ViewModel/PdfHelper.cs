using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace laundry.Models.ViewModel
{
    public class PdfHelper
    {
            public static void AddPrintFunction(string pdfPath, Stream outputStream)
            {
                PdfReader reader = new PdfReader(pdfPath);
                int pageCount = reader.NumberOfPages;
                Rectangle pageSize = reader.GetPageSize(1);

                // Set up Writer 
                PdfDocument document = new PdfDocument();

                PdfWriter writer = PdfWriter.GetInstance(document, outputStream);

                document.Open();

                //Copy each page 
                PdfContentByte content = writer.DirectContent;

                for (int i = 0; i < pageCount; i++)
                {
                    document.NewPage();
                    // page numbers are one based 
                    PdfImportedPage page = writer.GetImportedPage(reader, i + 1);
                    // x and y correspond to position on the page 
                    content.AddTemplate(page, 0, 0);
                }

                // Inert Javascript to print the document after a fraction of a second to allow time to become visible.
                string jsText = "var res = app.setTimeOut(‘var pp = this.getPrintParams();pp.interactive = pp.constants.interactionLevel.full;this.print(pp);’, 200);";

                //string jsTextNoWait = “var pp = this.getPrintParams();pp.interactive = pp.constants.interactionLevel.full;this.print(pp);”;
                PdfAction js = PdfAction.JavaScript(jsText, writer);
                writer.AddJavaScript(js);

                document.Close();

            }
    }
}