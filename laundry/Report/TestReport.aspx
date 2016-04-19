<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestReport.aspx.cs" Inherits="laundry.Report.TestReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div id="getR1">
            <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <div>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="100%" AsyncRendering="False" SizeToReportContent="True">

                </rsweb:ReportViewer>
            </div>
            <iframe id="frmPrint" name="IframeName"
                 runat="server"
                style="display: none"></iframe>
            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        </div>
    </form>
</body>
</html>
