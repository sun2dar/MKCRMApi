<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="NewCRMReport.Report.Index" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Src="~/UserControl/ReportView.ascx" TagPrefix="UserControl" TagName="ReportView" %>


<asp:Content ID="ContentReport" ContentPlaceHolderId="Content" runat="server">
    <script type="text/javascript">
        function printDiv(divID) {
            var divElements = document.getElementById(divID).innerHTML;                                         // Get the HTML of div
            var oldPage = document.body.innerHTML;                                                              // Get the HTML of whole page
            document.body.innerHTML = "<html><head><title></title></head><body>" + divElements + "</body>";     // Reset the page's HTML with div's HTML only
            window.print();                                                                                     // Print Page
            document.body.innerHTML = oldPage;                                                                  // Restore orignal HTML
        }

        $(document).ready(function () {

            function UpdateTargetLink() {
                hrefChangeTarget();
                setTimeout(function () { UpdateTargetLink(); }, 2000);
            }

            function hrefChangeTarget() {
                $("body").find("a").removeAttr("target", "_top");
                $("body").find("a").attr("target", "_blank");
            }

            UpdateTargetLink();
        });
    </script>

    <div class="row"><div class="col-sm-12 col-centered"></div></div>
    <div class="row">
        <div class="col-sm-12 col-centered">
            <asp:UpdatePanel ID="ReportPanel" runat="server">
                <ContentTemplate>
                    <asp:ScriptManager ID="ReportScript" runat="server"></asp:ScriptManager>
                    <div id="Reports-Location" class="report-location">
                        <UserControl:ReportView runat="server" ID="rpt" ReportTitle="Default" ReportName="Default Name" />
                        <rsweb:ReportViewer ID="myReportViewer" runat="server" HyperlinkTarget="_blank">
                            <LocalReport EnableHyperlinks="True">
                            </LocalReport>
                        </rsweb:ReportViewer>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>

</asp:Content>
