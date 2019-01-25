<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportView.ascx.cs" Inherits="NewCRMReport.UserControl.ReportView_ascx" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<style>
    .report-wrapper
    {
        background-color:White;
        width:95%; 
        height:100%; 
        margin-left: auto; 
        margin-right: auto; 
        margin-bottom: auto;
    }
</style>

<div class="report-wrapper">
    <rsweb:ReportViewer ID="ReportViewerMaster" 
        ShowBackButton="false"  
        ShowPrintButton="true" 
        ShowRefreshButton="true" 
        BorderWidth="2px" 
        runat="server" 
        Width="100%" 
        Height="100%" ProcessingMode="Local">
    </rsweb:ReportViewer>
</div>
    
