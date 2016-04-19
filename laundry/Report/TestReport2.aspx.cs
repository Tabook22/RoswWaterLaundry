using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System;
using System.IO;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace laundry.Report
{
    public partial class TestReport2 : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)

            {
                string billno = Request.QueryString["bno"]; //getting the bill no which are going to use it in the search 
                // BindReport();
                displayReport(billno);
                //printReport();
            }
        }

        //private void BindReport()
        //{
        //    try
        //    {
        //        ReportViewer1.ProcessingMode = ProcessingMode.Local;
        //        //report path
        //        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/RPTReports/Report1.rdlc");
        //        //
        //        SqlDataAdapter adp = new SqlDataAdapter("SELECT * FROM [Items]", con);
        //        //object of Dataset DsstudentDetail
        //       DataSet ds = new DataSet();
        //        adp.Fill(ds, "Items");
        //        //Datasource for report
        //        ReportDataSource datasource = new ReportDataSource("Datasert1", ds.Tables[0]);
        //        ReportViewer1.Width = 600;
        //        ReportViewer1.LocalReport.DataSources.Clear();
        //        ReportViewer1.LocalReport.DataSources.Add(datasource);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        private void displayReport(string billno)
        {
            DataSet ds = new DataSet();
            ds = GetDataRportDataset2(billno);
            //here we filter the records which stored inside table in the dataset 
            ReportDataSource myReportdatasource2 = new ReportDataSource();
            myReportdatasource2.Name = "DataSet2";
            //the name of the report datasource, we can check that if you go to the reprot desinger
            myReportdatasource2.Value = ds.Tables[0];
            // here our dataset has only one table therefore it is table No. 0
            //Set the processing mode for the ReportViewer to Local
            ReportViewer2.ProcessingMode = ProcessingMode.Local;
            // here we processing the report in the local mode , because we alos have the remote mode if we dealing with server
            //ReportViewer1.ZoomMode = ZoomMode.PageWidth 'for full width
            //report settings
            //ReportViewer2.Width = 350;
            //ReportViewer2.Height = 600;
            //clear the reportviewer datasource
            ReportViewer2.LocalReport.DataSources.Clear();

            ReportViewer2.LocalReport.ReportPath = Server.MapPath("~/RPTReports/Report3.rdlc");
            //Create the customer code number report parameter
            //ReportParameter clientCode = new ReportParameter();
            //clientCode.Name = "ParamNotes";
            // clientCode.Values.Add("المغسلة غير مسؤلة عن إي ملابس مر عليها 3 أشهر");
            //Set the report parameters for the report
            //ReportViewer1.LocalReport.SetParameters(clientCode);
            ReportViewer2.LocalReport.DataSources.Add(myReportdatasource2);
            ReportViewer2.LocalReport.Refresh();

            //print report
            // printReport();
        }

        private DataSet GetDataRportDataset2(string billno)
        {
            string strSql = "SELECT Bills.TransId,"
                + "Bills.CustId,"
                + "Bills.ItemId,"
                + "Bills.Date, "
                + "Bills.Qyt,"
                + "Bills.Cost,"
                + "Bills.BillNo,"
                + "Bills.printedBill,"
                + "Customers.CustName,"
                + "Customers.Tel, "
                + "Items.MId,"
                + "Items.SId, "
                + "Items.ItemName,"
                + "Items.itemImg,"
                + "Items.Price,"
                + "ItemMainCategories.catName "
                + "from Bills INNER JOIN Customers ON Bills.CustId = Customers.CustId INNER JOIN Items ON Bills.ItemId = Items.ItemId INNER JOIN  ItemMainCategories ON Items.MId = ItemMainCategories.Id where Bills.printedBill='" + billno + "' and catName='Dry Clean'";
            //+ "FROM Bills INNER JOIN Items ON Bills.ItemId = Items.ItemId INNER JOIN Customers ON Bills.CustId = Customers.CustId where Bills.printedBill='"+billno +"'";
            //+ "INNER JOIN Customers ON Bills.CustId = Customers.CustId "
            //+ "INNER JOIN Items ON Bills.ItemId = Items.ItemId "
            //+ "WHERE(Bills.printedBill =" +"(01)00168"+")";
            SqlCommand CMD2 = new SqlCommand(strSql, con);
            CMD2.Connection = con;
            SqlDataAdapter da2 = new SqlDataAdapter(CMD2);
            DataSet ds2 = new DataSet();
            try
            {
                con.Open();
                da2.Fill(ds2, "myBill2");
                return ds2;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                da2.Dispose();
                con.Close();
            }
        }

        private void printReport()
        {
            Warning[] warnings = null;
            string[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string extension = null;

            byte[] bytes = ReportViewer2.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);


            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/Report/nasser11.pdf"), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();


            //Open existing PDF
            Document document = new Document(PageSize.LETTER);
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/Report/nasser11.pdf"));
            //Getting a instance of new PDF writer
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("~/Report/print2.pdf"), FileMode.Create));
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
            PdfAction jAction = PdfAction.JavaScript("this.print(true);", writer);
            writer.AddJavaScript(jAction);
            document.Close();

            //Attach pdf to the iframe
            frmPrint.Attributes["src"] = "Print2.pdf";

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Warning[] warnings = null;
            string[] streamids = null;
            string mimeType = null;
            string encoding = null;
            string extension = null;

            byte[] bytes = ReportViewer2.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamids, out warnings);


            FileStream fs = new FileStream(HttpContext.Current.Server.MapPath("~/Report/nasser11.pdf"), FileMode.Create);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();


            //Open existing PDF
            Document document = new Document(PageSize.LETTER);
            PdfReader reader = new PdfReader(HttpContext.Current.Server.MapPath("~/Report/nasser11.pdf"));
            //Getting a instance of new PDF writer
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("~/Report/print2.pdf"), FileMode.Create));
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
            PdfAction jAction = PdfAction.JavaScript("this.print(true);", writer);
            writer.AddJavaScript(jAction);
            document.Close();

            //Attach pdf to the iframe
            frmPrint.Attributes["src"] = "Print2.pdf";
        }
    }
}