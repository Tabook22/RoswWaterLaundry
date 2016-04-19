<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptBills.aspx.cs" Inherits="laundry.myReports.rptBills" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>
<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <title>ReportViwer in MVC4 Application</title>    
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
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" AsyncRendering="false" SizeToReportContent="true">
        </rsweb:ReportViewer>        
<%--        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
        </rsweb:ReportViewer>--%>
    </div>
    </form>
</body>
</html>