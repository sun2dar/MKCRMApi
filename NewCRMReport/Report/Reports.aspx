<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="NewCRMReport.Report.Reports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="ContentReport" ContentPlaceHolderId="Content" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#ReportViewer").hide();
            var reportH = $(document).height();
            $("#Reports-Location").css("height", reportH - 150);
        });
    </script>

    <div class="row"><div class="col-sm-12 col-centered"></div></div>
    <div class="row">
        <div class="col-sm-12 col-centered">
            <asp:UpdatePanel ID="CRMUpdatePanel" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>
                    <div id="Reports-Location" class="report-location">
                        <rsweb:ReportViewer ID="CRMReportViewer" CssClass="report-content" runat="server" SizeToReportContent="true" AsyncRendering="true" Width="100%" ShowParameterPrompts="False" KeepSessionAlive="True"></rsweb:ReportViewer>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
 
</asp:Content>


