using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace laundry.Controllers
{
    public class testPrintingController : Controller
    {
        // GET: testPrinting
        public ActionResult Index()
        {
            //string htmlContent = RenderRazorViewToString("~/Views/Shared/testPDF.cshtml");

            //return File(GenerateHtmlToPDFDocument(htmlContent), "application/pdf");
            return RedirectToAction("prtR");
        }

        
        public void prtR()
        {
            using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10, 10, 10, 10);

                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                Chunk chunk = new Chunk("This is from chunk. ");
                document.Add(chunk);

                Phrase phrase = new Phrase("This is from Phrase.");
                document.Add(phrase);

                Paragraph para = new Paragraph("This is from paragraph.");
                document.Add(para);

                string text ="you are successfully created PDF file.";
                Paragraph paragraph = new Paragraph();
                paragraph.SpacingBefore = 10;
                paragraph.SpacingAfter = 10;
                paragraph.Alignment = Element.ALIGN_LEFT;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.GREEN);
                paragraph.Add(text);
                document.Add(paragraph);

                document.Close();
                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();
                Response.Clear();
                Response.ContentType = "application/pdf";

                string pdfName = "User";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + pdfName + ".pdf");
                Response.ContentType = "application/pdf";
                Response.Buffer = true;
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.BinaryWrite(bytes);
                Response.End();
                Response.Close();
            }
        }



        /// <summary>
        /// Render View to String
        /// </summary>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public string RenderRazorViewToString(string viewName)
        {
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Create chapter content from html
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public Chapter CreateChapterContent(string html)
        {
            // Declare a font to used for the bookmarks
            iTextSharp.text.Font bookmarkFont = FontFactory.GetFont(FontFactory.HELVETICA, 16, iTextSharp.text.Font.NORMAL);

            Chapter chapter = new Chapter(new Paragraph(""), 0);
            chapter.NumberDepth = 0;

            // Create css for some tag
            StyleSheet styles = new StyleSheet();

            styles.LoadTagStyle("h2", HtmlTags.ALIGN_MIDDLE, "center");
            styles.LoadTagStyle("h2", HtmlTags.COLOR, "#F90");
            styles.LoadTagStyle("pre", "size", "10pt");

            // Split H2 Html Tag
            string pattern = @"<\s*h2[^>]*>(.*?)<\s*/h2\s*>";
            string[] result = Regex.Split(html, pattern);

            // Create section title & content
            int sectionIndex = 0;
            foreach (var item in result)
            {
                if (string.IsNullOrEmpty(item)) continue;

                if (sectionIndex % 2 == 0)
                {
                    chapter.AddSection(20f, new Paragraph(item, bookmarkFont), 0);
                }
                else
                {
                    foreach (IElement element in HTMLWorker.ParseToList(new StringReader(item), styles))
                    {
                        chapter.Add(element);
                    }
                }

                sectionIndex++;
            }

            chapter.BookmarkTitle = "Demo for Load More Button in Kendo UI Grid";
            return chapter;
        }

        /// <summary>
        /// Generate PDF from HTML
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public byte[] GenerateHtmlToPDFDocument(string html)
        {
            MemoryStream workStream = new MemoryStream();
            Document pdfDoc = new Document(PageSize.A4);
            PdfWriter.GetInstance(pdfDoc, workStream).CloseStream = false;
            HTMLWorker parser = new HTMLWorker(pdfDoc);

            // Get chapter content
            Chapter chapter = CreateChapterContent(html);

            pdfDoc.Open();

            // Add chapter content to PDF
            pdfDoc.Add(chapter);

            pdfDoc.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            // Generate TOC for existing content
            return GeneratePDFTOCContent(byteInfo, html);
        }

        /// <summary>
        /// Generate PDF To Content
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public byte[] GeneratePDFTOCContent(byte[] content, string html)
        {
            var reader = new PdfReader(content);
            StringBuilder sb = new StringBuilder();

            // Title of PDF
            sb.Append("<h2><strong style='text-align:center'> Demo for Load More Button in Kendo UI Grid </ strong ></ h2 >< br > ");

            // Begin to create TOC
            // Begin to create TOC
            sb.Append("<table>");
            sb.Append(("<tr><td width='80%'><strong>{ 0}</strong ></td><td align = 'right' width = '10%'><strong >{1}</strong ></td></tr> "));
            using (MemoryStream ms = new MemoryStream())
            {
                // XML document generated by iText 
                SimpleBookmark.ExportToXML(SimpleBookmark.GetBookmark(reader), ms, "UTF-8", false);

                // rewind to create xmlreader
                ms.Position = 0;
                using (XmlReader xr = XmlReader.Create(ms))
                {
                    xr.MoveToContent();
                    string page = null;
                    string text = null;

                    string format = @"<tr><td width='80%'>{0}</td>
            <td align='right' width='10%'>{1}</td></tr>";

                    // extract page number from 'Page' attribute 
                    Regex re = new Regex(@"^\d+");
                    while (xr.Read())
                    {
                        if (xr.NodeType == XmlNodeType.Element &&
                        xr.Name == "Title" && xr.IsStartElement())
                        {
                            page = re.Match(xr.GetAttribute("Page")).Captures[0].Value;
                            xr.Read();

                            if (xr.NodeType == XmlNodeType.Text)
                            {
                                text = xr.Value.Trim();
                                int pageSection = int.Parse(page) + 1;
                                sb.Append(String.Format(format, text, pageSection.ToString()));
                            }
                        }
                    }
                }
            }

            sb.Append("</table>");

            MemoryStream workStream = new MemoryStream();
            var document = new Document(reader.GetPageSizeWithRotation(1));
            var writer = PdfWriter.GetInstance(document, workStream);
            writer.CloseStream = false;

            document.Open();
            document.NewPage();

            // Add TOC
            //StyleSheet styles = new StyleSheet();
            //styles.LoadTagStyle("h2", HtmlTags.ALIGN_MIDDLE, "center");
            //styles.LoadTagStyle("h2", HtmlTags.COLOR, "#F90");


            StyleSheet style = new StyleSheet();
            style.LoadTagStyle(HtmlTags.DIV, HtmlTags.WIDTH, "220px");
            style.LoadTagStyle(HtmlTags.DIV, HtmlTags.HEIGHT, "80px");
            style.LoadTagStyle(HtmlTags.DIV, HtmlTags.BGCOLOR, "@eeeeee");
            style.LoadStyle("address", "style", "font-size: 8px; text-align: justify; font-family: Arial, Helvetica, sans-serif;");
            style.LoadStyle("largeName", "style", "font-size: 10px; text-align: justify; font-family: Arial, Helvetica, sans-serif;");
            style.LoadStyle("description", "style", "font-size: 8px; text-align: justify; font-family: Arial, Helvetica, sans-serif;");


            foreach (IElement element in HTMLWorker.ParseToList(new StringReader(sb.ToString()), style))
            {
                document.Add(element);
            }

            // Append your chapter content again
            Chapter chapter = CreateChapterContent(html);
            document.Add(chapter);

            document.Close();
            writer.Close();

            byte[] byteInfo = workStream.ToArray();
            workStream.Write(byteInfo, 0, byteInfo.Length);
            workStream.Position = 0;

            return byteInfo;
        }
    }
}