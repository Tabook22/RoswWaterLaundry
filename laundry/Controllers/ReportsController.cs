using iTextSharp.text;
using iTextSharp.text.pdf;
using laundry.Models.DB;
using laundry.Models.ViewModel;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace laundry.Controllers
{
    //[CustomAuthorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private LundryDbContext db = new LundryDbContext();

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        ReportDataSource rd;
        LocalReport lr;

        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult view()
        {
           // pPDF();
            //SendToPrinter();
          //  SimpleSavePDF();
            //SendToPrinter();
            //myprint("(01)00168");
            return View();
        }

        public ActionResult billReport(string billno)
        {
            string id = "pdf";
            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/RPTReports"), "Report2.rdlc");
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
            else
            {
                return View("Index");
            }
            //List<Bill> cm = new List<Bill>();
            //List<Item> cm = new List<Item>();
            var cm = getCustBills(billno);
            // cm = db.Items.Where(x => x.MId == 1).ToList();
            //cm = db.Bills.Where(x => x.printedBill == "(01)00169").ToList();
            //cm = db.Items.ToList();

            ReportDataSource rd = new ReportDataSource("DataSet1", cm);
            lr.DataSources.Add(rd);
            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;


            //deviceInfo An XML string that contains the device-specific content that is required by the rendering extension specified in the format parameter.
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id + "</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

           return File(renderedBytes, mimeType);
           
        }
        public List<BillViewModel> getCustBills(string billno)
        {
            var getCsutBill = (from b in db.Bills
                               where b.printedBill==billno
                              join c in db.Customers on b.CustId equals c.CustId
                              join i in db.Items on b.ItemId equals i.ItemId
                              join mcat in db.ItemMainCategories on i.MId equals mcat.Id
                              select new BillViewModel
                              {
                                  CustId = b.CustId,
                                  ItemId = b.ItemId,
                                  Date = b.Date,
                                  Qyt = b.Qyt,
                                  Cost = b.Cost,
                                  printedBill = b.printedBill,
                                  CustName = c.CustName,
                                  Tel = c.Tel,
                                  ItemName = i.ItemName,
                                  itemImg = i.itemImg,
                                  catName =mcat.catName
                              }).ToList();
            //var getCsutBill = db.Bills.Where(x => x.printedBill == billno).
            //    Join(db.Customers,
            //   b => b.CustId, c => c.CustId,
            //    (b, c) => new
            //    {
            //        CustId = b.CustId,
            //        ItemId = b.ItemId,
            //        Date = b.Date,
            //        Qyt = b.Qyt,
            //        Cost = b.Cost,
            //        printedBill = b.printedBill,
            //        Customer = c.CustName,
            //        Tel = c.Tel
            //    }).Join(db.Items,
            //            a => a.ItemId, p => p.ItemId,
            //            (a, p) => new
            //            {
            //                CustId = a.CustId,
            //                ItemId = a.ItemId,
            //                Date = a.Date,
            //                Qyt = a.Qyt,
            //                Cost = a.Cost,
            //                printedBill = a.printedBill,
            //                Customer = a.Customer,
            //                Tel = a.Tel,
            //                ItemName = p.ItemName
            //            }).Select(bcp => new BillViewModel
            //            {
            //                CustId = bcp.CustId,
            //                ItemId = bcp.ItemId,
            //                Date = bcp.Date,
            //                Qyt = bcp.Qyt,
            //                Cost = bcp.Cost,
            //                printedBill = bcp.printedBill,
            //                CustName = bcp.Customer,
            //                Tel = bcp.Tel,
            //                ItemName = bcp.ItemName,
            //                itemImg = bcp.ItemName
            //            }).ToList();

            return getCsutBill;
        }

        //------------------------------------------------------------------------------------------
        //public ActionResult GetReport()
        //{
        //    try
        //    {
        //        GetReportPath();
        //        GetEmployeeList();
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Index");
        //    }
        //    return GetReportFile();
        //}
        //private string GetReportPath()
        //{
        //    lr = new LocalReport();
        //    string reportPath = null;
        //    reportPath = Path.Combine(Server.MapPath("~/RPTReports/Report1.rdlc"));
        //    return lr.ReportPath = reportPath;
        //}

        //private List<Item> GetEmployeeList()
        //{

        //    //var total = from T1 in db.Bills
        //    //            join T2 in db.Customers on T1.CustId equals T2.CustId
        //    //            join T3 in db.Items on T1.ItemId equals T3.ItemId
        //    //            group T3 by new { T1.CustId,
        //    //                T1.Date,
        //    //                T1.printedBill,
        //    //                T2.CustName,
        //    //                T1.Date,
        //    //            } into g
        //    //            select new
        //    //            {
        //    //                Column1 = g.Key.Column1,
        //    //                Column2 = g.Key.Column2,
        //    //                Amount = g.Sum(t3 => t3.Column1)
        //    //            };


        //    //            var model = db.Bills 
        //    //    .Where(r => r.printedBill== "(01)00169")
        //    //    .GroupBy(r => r.CustId)
        //    //    .Select(m => new BillViewModel
        //    //    {
        //    //CustId=m.Select(r=>r.CustId).First(),
        //    //ItemId=m.Select(r=>r.ItemId).First(),
        //    //Date =m.Select (r=>r.Date).First(),
        //    //        Qyt =m.Select(r=>r.Qyt).First(),
        //    //        Cost =m.Select(r=>r.Cost).First(),
        //    //        BillNo =m.Select(r=>r.BillNo).First(),
        //    //        printedBill =m.Select (r=>r.printedBill).First(),
        //    //        CustName =m.Select(r=>r.)
        //    //Tel=
        //    //ItemName =
        //    // itemImg =
        //    //Price=

        //    //        startDate = m.Max(r => r.StartDate),
        //    //        endDate = m.Max(r => r.EndDate),
        //    //        stafferId = m.Min(r => r.StafferId),
        //    //        startTime = m.Select(r => r.StartTime).First(),
        //    //        endTime = m.Select(r => r.EndTime).First()
        //    //    });



        //    //var employees = db.Bills.
        //    //    Join(db.Customers,
        //    //    o => o.CustId, od => od.CustId,
        //    //    (o, od) => new
        //    //    {
        //    //        CustId = o.CustId,
        //    //        ItemId = o.ItemId,
        //    //        Date = o.Date,
        //    //        Qyt = o.Qyt,
        //    //        Cost = o.Cost,
        //    //        BillNo = o.BillNo,
        //    //        printedBill = o.printedBill,
        //    //        Customer = od.CustName,
        //    //        Tel = od.Tel
        //    //    }).Join(db.Items,
        //    //            a => a.ItemId, p => p.ItemId,
        //    //            (a, p) => new
        //    //            {
        //    //                CustId = a.CustId,
        //    //                ItemId = a.ItemId,
        //    //                Date = a.Date,
        //    //                Qyt = a.Qyt,
        //    //                Cost = a.Cost,
        //    //                BillNo = a.BillNo,
        //    //                printedBill = a.printedBill,
        //    //                Customer = a.Customer,
        //    //                Tel = a.Tel,
        //    //                ItemName = p.ItemName
        //    //            }).Where(x => x.printedBill == "(01)00169").ToList();



        //    //var employees = db.Items.ToList();
        //    var employees = db.Items.Where(x => x.MId == 1).ToList();
        //    rd = new ReportDataSource("DataSet1", employees);
        //    lr.DataSources.Add(rd);
        //    return employees;
        //}

        //public ActionResult GetReportFile()

        //{
        //    string reportType = "pdf";
        //    string mimeType;
        //    string encoding;
        //    string fileNameExtension;
        //    string deviceInfo =
        //    "<DeviceInfo>" +
        //    "  <OutputFormat>" + reportType + "</OutputFormat>" +
        //    "  <PageWidth>11.5in</PageWidth>" +
        //    "  <PageHeight>8.30in</PageHeight>" +
        //    "  <MarginTop>0in</MarginTop>" +
        //    "  <MarginLeft>0in</MarginLeft>" +
        //    "  <MarginRight>0in</MarginRight>" +
        //    "  <MarginBottom>0in</MarginBottom>" +
        //    "</DeviceInfo>";
        //    Warning[] warnings;
        //    string[] streams;
        //    byte[] renderedBytes;
        //    renderedBytes = lr.Render(
        //        reportType,
        //        deviceInfo,
        //        out mimeType,
        //        out encoding,
        //        out fileNameExtension,
        //       out streams,
        //        out warnings);
        //    return File(renderedBytes, mimeType);
        //}


        //public ActionResult DepartmentwiseInwardOutwardReport(int? fd, int? td, string fdt, string tdt)
        public ActionResult DepartmentwiseInwardOutwardReport(string billno)
        {
            // Setup DataSet here we are going to set up the dataset and fill it with data
            var cm = getCustBills(billno);

            // Setup the report viewer object and get the array of bytes
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Server.MapPath("~/RPTReports/Report2.rdlc");

            // Create Report DataSource
            ReportDataSource reportDataSource = new ReportDataSource("DataSet1", cm);
            reportDataSource.Name = "DataSet1";
            localReport.DataSources.Add(reportDataSource);// Add datasource here

            // Variables
            string reportType = "PDF";
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>PDF</OutputFormat>" +
            "  <PageWidth>8.5in</PageWidth>" +
            "  <PageHeight>11in</PageHeight>" +
            "  <MarginTop>0.5in</MarginTop>" +
            "  <MarginLeft>1in</MarginLeft>" +
            "  <MarginRight>1in</MarginRight>" +
            "  <MarginBottom>0.5in</MarginBottom>" +
            "</DeviceInfo>";

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

           // Response.AddHeader("content-disposition", "attachment; filename=NorthWindCustomers." + fileNameExtension);
            return File(renderedBytes, mimeType);
        }

        public void myprint(string billno)
        {
            // Setup DataSet here we are going to set up the dataset and fill it with data
            var cm = getCustBills(billno);

            // Create Report DataSource
            ReportDataSource rds = new ReportDataSource("DataSet1", cm);


            // Variables
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;


            // Setup the report viewer object and get the array of bytes
            ReportViewer viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Server.MapPath("~/RPTReports/Report2.rdlc");
            viewer.LocalReport.DataSources.Add(rds); // Add datasource here


            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            // Now that you have all the bytes representing the PDF report, buffer it and send it to the client.
            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + "Report2" + "." + extension);
            Response.BinaryWrite(bytes); // create the file
            Response.Flush(); // send it to the client to download
        }

        public void SimpleSavePDF()
        {  
            // Setup DataSet here we are going to set up the dataset and fill it with data
            var cm = getCustBills("(01)00168");

            LocalReport report = new LocalReport();
            report.ReportPath = Server.MapPath("~/RPTReports/Report2.rdlc");

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";//This refers to the dataset name in the RDLC file
            rds.Value = cm;
            report.DataSources.Add(rds);
            //byte[] mybytes = report.Render("WORD");
            //byte[] mybytes = report.Render("EXCEL");
            Byte[] mybytes = report.Render("PDF"); //for exporting to PDF
            // using (FileStream fs = new FileStream(@"D:\SalSlip.doc", FileMode.Create))
            //using (FileStream fs = new FileStream(Server.MapPath("~/Report/nasser.doc"), FileMode.Create))
            using (FileStream fs = new FileStream(Server.MapPath("~/Report/nasser.pdf"), FileMode.Create))
            {
                fs.Write(mybytes, 0, mybytes.Length);
            }
        }


        //sends pdf to printer
        private void SendToPrinter()
        {
            FileStream fs = new FileStream(Server.MapPath("~/Report/nasser7.pdf"), FileMode.Create);
            PdfReader reader = new PdfReader(Server.MapPath("~/Report/nasser.pdf"));
            int pageCount = reader.NumberOfPages;
            Rectangle pageSize = reader.GetPageSize(1);

            // Set up Writer 
            Document document = new Document(pageSize);
            // PdfDocument document = new PdfDocument(pageSize);

            PdfWriter writer = PdfWriter.GetInstance(document, fs);

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


            // FileStream fs = new FileStream(Server.MapPath("~/Report/nasser6.pdf"), FileMode.Create);
            // PdfReader reader = new PdfReader(Server.MapPath("~/Report/nasser.pdf"));
            // int pageCount = reader.NumberOfPages;
            // Rectangle pageSize = reader.GetPageSize(1);
            // // Setup writer
            // Document document = new Document(pageSize);
            // // PdfDocument document = new PdfDocument();
            // PdfWriter writer = PdfWriter.GetInstance(document, fs);
            // document.Open();
            // // Copy each page
            // PdfContentByte content = writer.DirectContent;
            // for (int i = 0; i < pageCount; i++)
            // {
            //     document.NewPage();
            //     PdfImportedPage page = writer.GetImportedPage(reader, i + 1); // page numbers are one-based
            //     content.AddTemplate(page, 0, 0); // x and y correspond to position on the page?
            // }
            // // Insert JavaScript to print the document after a fraction of a second (allow time to become visible).
            //// string jsText = "var res = app.setTimeOut('var pp = this.getPrintParams();pp.interactive = pp.constants.interactionLevel.full;this.print(pp);', 200);";
            // string jsTextNoWait = "var pp = this.getPrintParams();pp.interactive = pp.constants.interactionLevel.full;this.print(pp);";
            // PdfAction js = PdfAction.JavaScript(jsTextNoWait, writer);
            // writer.AddJavaScript(js);
            // document.Close();
        }

        //display pdf to the screen
        public ActionResult GetPdf(string fileName)
        {
            var fileStream = new FileStream(Server.MapPath("~/Report/"+"nasser7.pdf"),
                                             FileMode.Open,
                                             FileAccess.Read
                                           );
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            return fsResult;
        }

        public void pPDF()

        {

            //Open existing PDF
            Document document = new Document(PageSize.LETTER);
            PdfReader reader = new PdfReader(Server.MapPath("~/Report/nasser7.pdf"));

            //Getting a instance of new PDF writer
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~/Report/nasser12.pdf"), FileMode.Create));
            document.Open();
            PdfContentByte cb = writer.DirectContent;

            int i = 0;
            int p = 0;
            int n = reader.NumberOfPages;
            Rectangle psize = reader.GetPageSize(1);

            float width = psize.Width;
            float height = psize.Height;

            //Add Page to new document
            while (i < n)
            {
                document.NewPage();
                p += 1;
                i += 1;

                PdfImportedPage page1 = writer.GetImportedPage(reader, i);
                cb.AddTemplate(page1, 0, 0);
            }

            //Attach javascript to the document
            PdfAction jAction = PdfAction.JavaScript("this.print(true);\r", writer);
            writer.AddJavaScript(jAction);
            document.Close();

            //Attach pdf to the iframe
            //frmPrint.Attributes("src") = "Print2.pdf";
        }

        //opeing PDF inside iframe
        public FileStreamResult GetPdfPages(string id)
        {
            var pParams = id.Split('¬');

            var fileName = pParams[0];
            var start = Convert.ToInt32(pParams[1]);
            var end = Convert.ToInt32(pParams[2]);

            var inputFile = Server.MapPath(@"~/Report/" + fileName + ".pdf");

            var inputPdf = new PdfReader(inputFile);

            int pageCount = inputPdf.NumberOfPages;

            if (end < start || end > pageCount)
            {
                end = pageCount;
            }

            var inputDoc =
              new Document(inputPdf.GetPageSizeWithRotation(1));

            using (MemoryStream ms = new MemoryStream())
            {

                var outputWriter = PdfWriter.GetInstance(inputDoc, ms);
                inputDoc.Open();
                var cb1 = outputWriter.DirectContent;

                for (var i = start; i <= end; i++)
                {
                    inputDoc.SetPageSize(inputPdf.GetPageSizeWithRotation(i));
                    inputDoc.NewPage();

                    var page =
                      outputWriter.GetImportedPage(inputPdf, i);
                    int rotation = inputPdf.GetPageRotation(i);

                    if (rotation == 90 || rotation == 270)
                    {
                        cb1.AddTemplate(page, 0, -1f, 1f, 0, 0,
                                        inputPdf.GetPageSizeWithRotation(i).Height);
                    }
                    else
                    {
                        cb1.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                    }
                }

                inputDoc.Close();

                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "inline;test.pdf");
                Response.Buffer = true;
                Response.Clear();
                Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                Response.OutputStream.Flush();
                Response.End();

                return new FileStreamResult(Response.OutputStream, "application/pdf");
            }

        }
    }
}