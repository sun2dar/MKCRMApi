<%@ Page Title="Report Server is Working" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="NewCRMReport.Index" %>

<asp:Content ID="ContentErorr" ContentPlaceHolderId="Content" runat="server">

<style>
    .padhome
    {
        padding : 0px !important;
        border-color: white !important;
    }

    #padhomeinfo
    {
        padding : 15px !important;
    }

    .remove-padding
    {
        padding: 0px !important;
    }

    .width951
    {
        width: 951px !important;
    }
</style>

<br />
<br />
<br />

<div class="row">
    <div class="col-md-12 col-centered">
        <div class="row width951 col-centered">
            <div class="col-md-12 remove-padding">
                <div class="panel panel-default" style="overflow:hidden;">
                    <div class="panel-heading text-center padhome">
                        <img src="../images/homeup.png" alt="homeup"/>
                    </div>
                        
                    <div id="padhomeinfo" class="panel-body">
                        <div class="row">
                            <div class="col-md-1"></div>
                            <div class="col-md-10">
                                <div class="form-horizontal">
                                    <div class="form-group margin-bot-0">
                                        <div class="col-sm-4">
                                            <img src="../images/BCALOGOBIG.png" alt="bg" height="75"/>
                                        </div>
                                        <div class="col-sm-8">
                                            <h1>Aplikasi Report BCA CRM</h1>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-1"></div>
                        </div>
                
                    </div>

                    <div class="panel-footer text-center padhome">
                        <img src="../images/homedown.png" alt="homedown" height="200"/>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
</asp:Content>