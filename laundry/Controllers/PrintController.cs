//using iTextSharp.text;
//using iTextSharp.text.pdf;
//using laundry.Models.DB;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Web.Helpers;
//using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Helpers;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using laundry.Models.DB;
using iTextSharp.text.pdf.codec;
using iTextSharp.text.html.simpleparser;
using Microsoft.AspNet.Identity;
using System.Web.Services.Description;

namespace laundry.Controllers
{
    public class PrintController : Controller
    {
        private LundryDbContext db = new LundryDbContext();
        // GET: Print
        public ActionResult Index()
        {
            //ok..this is now a new changes
            return View(db.Customers.ToList());
        }

        public ActionResult GeneratePdf()
        {
            var getUers = db.Customers.OrderBy(x => x.CustName).ToList();
            return View(getUers);
        }

        public ActionResult printPDF()
        {

            string pdfBody = string.Empty;
            pdfBody += "<table>";
            pdfBody += "<tr>";
            pdfBody += "<td> C </ td >";
            pdfBody += "<td> RoseWater</td>";
            pdfBody += "<td></td>";
            pdfBody += "</tr>";
            pdfBody += "<tr>";
            pdfBody += "<td>Date:01/06/201</td>";
            pdfBody += "<td>Name:Nasser</td>";
            pdfBody += "<td>Te:1234566</td>";
            pdfBody += "</tr>";
           

            foreach(var itm in db.Customers.OrderBy(x=>x.CustName).ToList())
            {
                pdfBody += "<tr>";
                pdfBody += "<td>"+itm.CustName +"</td>";
                pdfBody += "<td>"+itm.Tel+"</td>";
                pdfBody += "<td>"+itm.CustId +"</td>";
                pdfBody += "</tr>";
            }
            pdfBody += "</table>";
            Document document = new Document();
            //string filenm = "UserList.pdf";
            string filenm = "BillNo-" + DateTime.Now.Ticks + ".pdf";
            var fpath = HttpContext.Server.MapPath("~/Documents/PDFDocuments/");
            //PngWriter.GetInstance(document, new FileStream(fpath + filenm, FileMode.Create));

            PdfWriter oPdfWriter = PdfWriter.GetInstance(document, new FileStream(fpath + filenm, FileMode.Create));

            document.Open();
            //If you want to add phrase:

            Phrase titl = new Phrase("\nCustomer Copy\n");
            titl.Font.SetStyle(Font.BOLD);
            document.Add(titl);

            iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();

            iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

            hw.Parse(new StringReader(pdfBody));
            var logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Content/images/logo.png"));
            logo.SetAbsolutePosition(300, 750);
            document.Add(logo);

            string jsText = "var res = app.setTimeOut(‘var pp = this.getPrintParams();pp.interactive = pp.constants.interactionLevel.full;this.print(pp);’, 200);";

            PdfAction js = PdfAction.JavaScript(jsText, oPdfWriter);
            oPdfWriter.AddJavaScript(js);

            document.Close();

            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "inline;filename=" + filenm + "");
            Response.ContentType = "application/pdf";

            Response.WriteFile(HttpContext.Server.MapPath("~/Documents/PDFDocuments/") + filenm);
            Response.Flush();
            Response.Clear();

            //printMyPDF("UserList.pdf");
            ViewBag.myPDF = filenm;
            return View();
        }


        //public byte[] GetPDF(string pHTML)
        //{
        //    byte[] bPDF = null;

        //    MemoryStream ms = new MemoryStream();
        //    TextReader txtReader = new StringReader(pHTML);

        //    // 1: create object of a itextsharp document class
        //    Document doc = new Document(PageSize.A4, 25, 25, 25, 25);

        //    // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file
        //    PdfWriter oPdfWriter = PdfWriter.GetInstance(doc, ms);

        //    // 3: we create a worker parse the document
        //    HTMLWorker htmlWorker = new HTMLWorker(doc);

        //    // 4: we open document and start the worker on the document
        //    doc.Open();
        //    htmlWorker.StartDocument();

        //    // 5: parse the html into the document
        //    htmlWorker.Parse(txtReader);

        //    // 6: close the document and the worker
        //    htmlWorker.EndDocument();
        //    htmlWorker.Close();
        //    doc.Close();

        //    bPDF = ms.ToArray();

        //    return bPDF;
        //}

        //public void DownloadPDF()
        //{
        //    string HTMLContent = "Hello <b>World</b>";

        //    Response.Clear();
        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=" + "PDFfile.pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.BinaryWrite(GetPDF(HTMLContent));
        //    Response.End();
        //}

    }
}