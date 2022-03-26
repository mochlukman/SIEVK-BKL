<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewAPBDEvaluasiPerKegiatan2.aspx.cs" Inherits="SIEVK.Pemda.Bengkalis.Views.ViewAPBDEvaluasiPerKegiatan2" %>
 
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
 
<%--<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>--%>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
    


<!DOCTYPE html> <%--PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"--%>
 
 <head runat="server">
     <meta name="viewport" content="width-device-width" />
     <title>Data Report</title>
 </head>
  
<body>  
  <form id="form1" runat="server">
    
    <%--<asp:scriptmanager id="ScriptManager1" runat="server"></asp:scriptmanager>    
       <div>

            <rsweb:reportViewer id="rptViewer" runat="server" height="100%" width="100%" borderstyle="None" clientidmode="AutoID" pagecountmode="Actual" processingmode="Remote" viewstatemode="Enabled" internalborderstyle="Ridge" waitcontroldisplayafter="10" showdocumentmapbutton="False">
            </rsweb:reportViewer>
       </div>--%>
      <CR:CrystalReportViewer ID="rptViewer" runat="server" AutoDataBind="true" />
  </form>
</body>
