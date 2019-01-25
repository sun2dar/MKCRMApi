using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Principal;
using System.IO;
using NLog;
using NLog.Targets;

namespace NewCRMReport.Report
{
    public partial class Reports : System.Web.UI.Page
    {
        private Logger log = NLog.LogManager.GetCurrentClassLogger();

        string username = ConfigurationManager.AppSettings["username"].ToString();
        string password = ConfigurationManager.AppSettings["password"].ToString();
        string domain = ConfigurationManager.AppSettings["domain"].ToString();

        string rdl = string.Empty;
        List<ReportParameter> param = new List<ReportParameter>();
        string BU = string.Empty;
        string usr = string.Empty;
        string[] keys;

        protected void Page_Init(object sender, EventArgs e)
        {
            SetupParameter();
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetupReport();
                RunReport();
            }
        }

        protected void SetupParameter() {
            string message = "\r\n";
            int n = 0;
            keys = Request.QueryString.AllKeys;
            foreach (string key in keys)
            {
                if ("rdl".Equals(key))
                {
                    this.rdl = string.IsNullOrEmpty(Request.QueryString[key]) ? string.Empty : Request.QueryString[key];
                    message += "RDL           : " + this.rdl + "\r\n";
                }
                else if ("BU".Equals(key))
                {
                    this.BU = string.IsNullOrEmpty(Request.QueryString[key]) ? string.Empty : Request.QueryString[key];
                    message += "Business Unit : " + this.BU + "\r\n";
                }
                else if ("USR".Equals(key))
                {
                    this.usr = string.IsNullOrEmpty(Request.QueryString[key]) ? string.Empty : Request.QueryString[key];
                    message += "User          : " + this.usr + " -> " + DateTime.Now.ToString() + "\r\n";
                }
                else
                {
                    message += "Parameter " + n++ + "   : " + key + " -> " + Request.QueryString[key].ToString() + "\r\n";
                    param.Add(new ReportParameter(key, Request.QueryString[key]));
                }
            }

            if (!string.IsNullOrEmpty(this.rdl) && 
                ("Aktivitas_SA".Equals(this.rdl)
                || "User_Per_Business_Unit".Equals(this.rdl)
                || "User_Profile_CRM".Equals(this.rdl)))
            {
                this.BU = "BUS".Equals(this.BU) ? "Business Unit / Branch Support" : this.BU;
                param.Add(new ReportParameter("BusinessUnitName", this.BU));
            }
            log.Info(message);
        }

        protected void SetupReport() {
            CRMReportViewer.ServerReport.ReportServerCredentials = new RptFunction(username, password, domain);
            CRMReportViewer.ProcessingMode = ProcessingMode.Remote;
            CRMReportViewer.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServer"].ToString());
            CRMReportViewer.ServerReport.ReportPath = ConfigurationManager.AppSettings["ReportFolder"].ToString() + rdl;
            CRMReportViewer.ServerReport.SetParameters(param);
        }

        protected void RunReport() {
            CRMReportViewer.ServerReport.Refresh();
            CRMUpdatePanel.Update();
        }
    }

    public class RptFunction : IReportServerCredentials
    {
        private string _userName;
        private string _password;
        private string _domain;

        public RptFunction(string userName, string password, string domain)
        {
            _userName = userName;
            _password = password;
            _domain = domain;
        }

        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }

        public ICredentials NetworkCredentials
        {
            get
            {
                return new NetworkCredential(_userName, _password, _domain);
            }
        }

        public bool GetFormsCredentials(out Cookie authCookie, out string user, out string password, out string authority)
        {
            authCookie = null;
            user = password = authority = null;
            return false;
        }
    }
}