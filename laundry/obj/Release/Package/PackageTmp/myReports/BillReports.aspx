<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillReports.aspx.cs" Inherits="laundry.myReports.BillReports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <script runat="server">
        void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<laundry.Models.DB.Bill> bill = null;
                using (laundry.Models.DB.LundryDbContext dc = new laundry.Models.DB.LundryDbContext())
                {
                    bill = dc.Bills.OrderBy(a => a.printedBill).ToList();
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/RPTReports/rptBills.rdlc");
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportDataSource rdc = new ReportDataSource("myDataset", bill);
                    ReportViewer1.LocalReport.DataSources.Add(rdc);
                    ReportViewer1.LocalReport.Refresh();
                }
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
         <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" SizeToReportContent="true">
        </rsweb:ReportViewer>        
    </div>
    </form>
</body>
</html>
