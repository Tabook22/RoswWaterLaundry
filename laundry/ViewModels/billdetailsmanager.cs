
using iTextSharp.text;
using iTextSharp.text.pdf;
using laundry.Models.DB;
using laundry.Security;
using System;
using System.IO;
using System.Linq;
using System.Web;

namespace laundry.ViewModels
{
    public class billdetailsmanager
    {
        private LundryDbContext db = new LundryDbContext();
        //TODO: Add new Bill----------------------------------------------------------------
        public void AddNewBill(AddNewBill bdv)
        {

            //hear we are going to get the max no form YearMaxBillNo., why we do that because that no will be used to form the finall bill no.
            int maxNo = 0;
            using (LundryDbContext db = new LundryDbContext())
            {
                if (checkDates()) //if the current date is less than the end date which is the last day in the year e.g (31/12/2016)
                {
                    //TODO: if result is true...get max from the current year;
                    var getCYear = DateTime.Now.Year.ToString();
                    maxNo = getMaxBillNo(getCYear); //get the max bill no depending on the year from table yearmaxbillnoes
                    AddBill(maxNo); //calling the method AddBill to add the new bill to the database

                    //update the YearMaxBillNoes by adding the new max bill no to the year
                   
                    int getCount = db.yearmaxbillno.Where(x => x.year.Equals(getCYear)).Count();

                    if (getCount >0) // check to see if we are not at the begining of the year
                    {
                        YearMaxBillNo yrbl = db.yearmaxbillno.Where(x => x.year.Equals(getCYear)).FirstOrDefault();
                        yrbl.maxbillno = (maxNo + 1);
                        db.SaveChanges();

                    }
                    else
                    {
                        YearMaxBillNo yrbl = new YearMaxBillNo();
                        yrbl.year = getCYear;
                        yrbl.maxbillno = 1;
                        db.yearmaxbillno.Add(yrbl);
                        db.SaveChanges();

                        
                    }
                }
                else
                {
                    //TODO: if result is true...get max from the current year;
                    var getCYear = DateTime.Now.Year.ToString();
                    maxNo = getMaxBillNo(getCYear); //get the max bill no depending on the year from table yearmaxbillnoes
                    AddBill(maxNo); //calling the method AddBill to add the new bill to the database

                    //update the YearMaxBillNoes by adding the new max bill no to the year
                    YearMaxBillNo yrbl = db.yearmaxbillno.Where(x => x.year.Equals(getCYear)).FirstOrDefault();
                    if (string.IsNullOrEmpty(yrbl.year))
                    {
                        yrbl.maxbillno = maxNo + 1;
                        db.SaveChanges();

                    }
                    else
                    {
                        yrbl.year = getCYear;
                        yrbl.maxbillno = 1;
                        db.SaveChanges();
                    }
                }
            }


        }

        //TODO: Check to see of there is a bill has the same bill no-----------------------------------
        public bool IsBillExist(int billno)
        {
            using (LundryDbContext db = new LundryDbContext())
            {
                return db.paidbills.Where(x => x.BillNo.Equals(billno)).Any(); // the Any() checks at least for one match and it return true if it found one, or false if it not
            }
        }

        //Check to see if the two dates are equals--------------------------------------------------
        public bool checkDates()
        {
            bool dtDiff = false;
            //Rquired Date Format
            String format = "dd/MM/yyyy";

            //Get the current date
            var currentDate = DateTime.Now.ToString("dd/MM/yyyy");
            var getCYear = DateTime.Now.Year.ToString();

            //EndDate of the year
            var endDate = "31/12/" + getCYear;
            //currentDate = "01/10/2017";
            DateTime dt1 = DateTime.ParseExact(currentDate.ToString(), format, System.Globalization.CultureInfo.InvariantCulture);
            DateTime dt2 = DateTime.ParseExact(endDate.ToString(), format, System.Globalization.CultureInfo.InvariantCulture);
            //find the difference
            if (dt1.Date < dt2.Date)
            {
                YearMaxBillNo yrbl = new YearMaxBillNo();
                //yrbl.maxbillno=
                dtDiff = true;
            }
            return dtDiff;
        }

        //Get the Maximum bill Nol---------------------------------------------------------------------------
        public int getMaxBillNo(string yr)
        {
            int maxBNo = 0;
            using (LundryDbContext db = new LundryDbContext())
            {
                // get the maximum no for yearmaxbillno
                //This is add 0 if the result of sum is null, because if there is no previous bill added before, then the result of the sum is null to prevent that we add 0
               int getCount= db.yearmaxbillno.Where(x => x.year.Equals(yr)). Select(x => new { maxbillno = x.maxbillno }).Count();
                if(getCount >0)
                {
                    maxBNo = db.yearmaxbillno.Where(x => x.year.Equals(yr)).Select(x => new { maxbillno = x.maxbillno }).FirstOrDefault().maxbillno;
                }
                else
                    {
                    maxBNo = 0;
                }
                return maxBNo;
            }
        }

        public void AddBill(int maxBillNo)
        {
            using (LundryDbContext db = new LundryDbContext())
            {
                var getCYear = DateTime.Now.Year.ToString();//get current year
                getCYear = getCYear.Substring(2);
                Bill bl = new Bill();
                paidbill pbl = new paidbill();
                //here am searching inside the temp table to get all the items qyt and price, which were perviouslly added before saving the whole bill details
                var orr = (from s in db.tempBills
                           select s).ToList();

                foreach (var itm in orr)
                {
                    bl.ItemId = itm.ItemId;
                    bl.CustId = itm.CustId;
                    bl.Qyt = itm.Qyt;
                    bl.Cost = itm.Cost;
                    String format = "dd/MM/yyyy";
                    var currentDate = DateTime.Today.ToString("dd/MM/yyyy");
                    DateTime dt = DateTime.ParseExact(currentDate.ToString(), format, System.Globalization.CultureInfo.InvariantCulture);
                    //  DateTime dte = DateTime.ParseExact(dd, format, CultureInfo.InvariantCulture);
                    bl.Date = dt;
                    bl.BillNo = maxBillNo + 1;
                    bl.printedBill = "(0" + SessionPersister.BranchID + ")" + (getCYear+(maxBillNo + 1)).ToString().PadLeft(5, '0') ; // this is the bill no which will be printed

                    db.Bills.Add(bl);
                    db.SaveChanges();
                }
                //Here we are going to remove all the items from the tempBills table
                foreach (var itm in orr)
                {
                    db.tempBills.Remove(itm);
                }
                // now we are going to add the new entered bill inot the paidbill tables, and makred it unpaid
                pbl.BillNo = maxBillNo + 1;
                pbl.printedBill = "(0" + SessionPersister.BranchID + ")" + (getCYear + (maxBillNo + 1)).ToString().PadLeft(5, '0'); // this is the bill no which will be printed
                pbl.IsPaid = false;

                db.paidbills.Add(pbl);
                db.SaveChanges();
                SessionPersister.printBillNo = "(0" + SessionPersister.BranchID + ")" + (getCYear + (maxBillNo + 1)).ToString().PadLeft(5, '0');
            }
        }

        //print bill
        //public void printPDF()
        //{

        //    string pdfBody = string.Empty;
        //    pdfBody += "<table>";
        //    pdfBody += "<tr>";
        //    pdfBody += "<td> C </ td >";
        //    pdfBody += "<td> RoseWater</td>";
        //    pdfBody += "<td></td>";
        //    pdfBody += "</tr>";
        //    pdfBody += "<tr>";
        //    pdfBody += "<td>Date:01/06/201</td>";
        //    pdfBody += "<td>Name:Nasser</td>";
        //    pdfBody += "<td>Te:1234566</td>";
        //    pdfBody += "</tr>";


        //    foreach (var itm in db.Customers.OrderBy(x => x.CustName).ToList())
        //    {
        //        pdfBody += "<tr>";
        //        pdfBody += "<td>" + itm.CustName + "</td>";
        //        pdfBody += "<td>" + itm.Tel + "</td>";
        //        pdfBody += "<td>" + itm.CustId + "</td>";
        //        pdfBody += "</tr>";
        //    }
        //    pdfBody += "</table>";
        //    Document document = new Document();
        //    //string filenm = "UserList.pdf";
        //    string filenm = "BillNo-" + DateTime.Now.Ticks + ".pdf";
        //    var fpath = HttpContext.Server.MapPath("~/Documents/PDFDocuments/");
        //    //PngWriter.GetInstance(document, new FileStream(fpath + filenm, FileMode.Create));

        //    PdfWriter oPdfWriter = PdfWriter.GetInstance(document, new FileStream(fpath + filenm, FileMode.Create));

        //    document.Open();
        //    //If you want to add phrase:

        //    Phrase titl = new Phrase("\nCustomer Copy\n");
        //    titl.Font.SetStyle(Font.BOLD);
        //    document.Add(titl);

        //    iTextSharp.text.html.simpleparser.StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();

        //    iTextSharp.text.html.simpleparser.HTMLWorker hw = new iTextSharp.text.html.simpleparser.HTMLWorker(document);

        //    hw.Parse(new StringReader(pdfBody));
        //    var logo = iTextSharp.text.Image.GetInstance(HttpContext.Server.MapPath("~/Content/images/logo.png"));
        //    logo.SetAbsolutePosition(300, 750);
        //    document.Add(logo);

        //    string jsText = "var res = app.setTimeOut(‘var pp = this.getPrintParams();pp.interactive = pp.constants.interactionLevel.full;this.print(pp);’, 200);";

        //    PdfAction js = PdfAction.JavaScript(jsText, oPdfWriter);
        //    oPdfWriter.AddJavaScript(js);

        //    document.Close();

        //    //Response.ClearContent();
        //    //Response.ClearHeaders();
        //    //Response.AddHeader("Content-Disposition", "inline;filename=" + filenm + "");
        //    //Response.ContentType = "application/pdf";

        //    //Response.WriteFile(HttpContext.Server.MapPath("~/Documents/PDFDocuments/") + filenm);
        //    //Response.Flush();
        //    //Response.Clear();

        //    //printMyPDF("UserList.pdf");

        //}


    }
}