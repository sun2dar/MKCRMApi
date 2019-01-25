using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NewCRMReport.UserControl;
using System.Reflection;
using NewCRMReport.Model;
using System.Data.SqlClient;
using System.Data.Objects;
using System.Linq.Dynamic;
using Microsoft.Reporting.WebForms;
using NLog;
using NLog.Targets;
using System.Configuration;
using System.Collections;


namespace NewCRMReport.Report
{
    public class ReportEntity
    {
        public IQueryable<Request> Requests { get; set; }
        public IQueryable<Task> Tasks { get; set; }
        public IQueryable<SystemUser> Users { get; set; }
        public IQueryable<CallWrapUpReport> CWU { get; set; }
    }

    public partial class Index : System.Web.UI.Page
    {
        private Logger log = NLog.LogManager.GetCurrentClassLogger();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    string query = Request.Url.Query;
                    int getDividerIdx = query.IndexOf("&");
                    string entity = query.Substring(1, getDividerIdx - 1);
                    string _params = query.Substring(getDividerIdx + 1);

                    log.Info(query);

                    ReportBinding(_params, entity);
                }
            }
            catch (Exception ex) {
                log.Error(ex.ToString());
            }
        }

        public void ReportBinding(string _params, string entity)
        {
            using (BCA_MSCRMEntities context = new BCA_MSCRMEntities())
            {
                try
                {
                    List<List<string>> AllFilters = new List<List<string>>();
                    string[] Parameters = _params.Split('&');
                    for (int i = 0; i < Parameters.Length - 4; i++)
                    {
                        string[] entry = Parameters[i].Split('=');
                        if (entry[2].Contains(','))
                        {
                            string[] filterValues = entry[2].Split(',');
                            for (int j = 0; j < filterValues.Length; j++)
                            {
                                List<string> filter = new List<string>();
                                filter.Add(entry[0]);
                                filter.Add(entry[1]);
                                filter.Add(filterValues[j]);
                                AllFilters.Add(filter);
                            }
                        }
                        else
                        {
                            List<string> filter = new List<string>();
                            filter.Add(entry[0]);
                            filter.Add(entry[1]);
                            filter.Add(entry[2]);
                            AllFilters.Add(filter);
                        }
                    }

                    ReportEntity reportEntitiy = new ReportEntity();
                    reportEntitiy.Requests = from req in context.Requests select req;
                    reportEntitiy.Tasks = from task in context.Tasks select task;
                    reportEntitiy.Users = from user in context.SystemUsers select user;
                    reportEntitiy.CWU = from cwu in context.CallWrapUpReports select cwu;

                    GenerateReport(AllFilters, Parameters, reportEntitiy, entity);
                }
                catch (Exception ex)
                {
                    log.Error(ex.ToString());
                }
            }
        }

        public void GenerateReport(List<List<string>> Filters, string[] Parameters, ReportEntity reportEntitiy, string entity)
        {
            string lastFilterName = "";
            string lastFilterType = "";
            List<string> filterName = new List<string>(); //eg. CategoryID, CreatedOn, ProductID, StatusCode
            List<string> filterType = new List<string>(); //eg. guid, int, string, datetime

            // Store distinct Filters name to filterName
            for (int i = 0; i < Filters.Count; i++)
            {
                if (Filters[i][0] != lastFilterName)
                {
                    filterName.Add(Filters[i][0]);
                    filterType.Add(Filters[i][1]);
                }
                else if (Filters[i][1] != lastFilterType)
                {
                    filterName.Add(Filters[i][0]);
                    filterType.Add(Filters[i][1]);
                }
                lastFilterName = Filters[i][0];
                lastFilterType = Filters[i][1];
            }

            IList<Guid?> filterIds = new List<Guid?>();
            IList<int> filterCodes = new List<int>();

            int startingIndex = 0;
            int filterCounter = 0;
            for (int i = 0; i < filterName.Count; i++ )
            {
                filterIds = new List<Guid?>();
                filterCodes = new List<int>();

                //Grouping ids with the same filter name eg. CategoryID(xxx-xxx-xxx, xxx-xxx-xxx)
                for (int j = startingIndex; j < Filters.Count; j++)
                {
                    if (Filters[j][0] == filterName[i])
                    {
                        switch (Filters[j][1])
                        {
                            case "guid":
                                filterIds.Add(new Guid(Filters[j][2]));
                                break;
                            case "number":
                                filterCodes.Add(int.Parse(Filters[j][2]));
                                break;
                        }
                        if (Filters[j][1] != "enddatetime")
                            filterCounter++;
                    }
                }

                switch (filterType[i])
                {
                    case "guid":
                        switch (entity)
                        {
                            case "Request":
                                reportEntitiy.Requests = reportEntitiy.Requests.Where("@0.Contains(outerIt." + filterName[i] + ")", filterIds);
                                break;
                            case "Task":
                                reportEntitiy.Tasks = reportEntitiy.Tasks.Where("@0.Contains(outerIt." + filterName[i] + ")", filterIds);
                                break;
                            case "SystemUser":
                                reportEntitiy.Users = reportEntitiy.Users.Where("@0.Contains(outerIt." + filterName[i] + ")", filterIds);
                                break;
                            case "CallWrapUp":
                                reportEntitiy.CWU = reportEntitiy.CWU.Where("@0.Contains(outerIt." + filterName[i] + ")", filterIds);
                                break;
                        }
                        break;
                    case "number":
                        string tempQueryString = "";
                        for (int code = 0; code < filterCodes.Count; code++)
                        {
                            if (code == 0)
                                tempQueryString = filterName[i] + "=" + filterCodes[code];
                            else
                                tempQueryString += "||" + filterName[i] + "=" + filterCodes[code];
                        }
                        switch (entity)
                        {
                            case "Request":
                                reportEntitiy.Requests = reportEntitiy.Requests.Where(tempQueryString);
                                break;
                            case "Task":
                                reportEntitiy.Tasks = reportEntitiy.Tasks.Where(tempQueryString);
                                break;
                            case "SystemUser":
                                reportEntitiy.Users = reportEntitiy.Users.Where(tempQueryString);
                                break;
                            case "CallWrapUp":
                                reportEntitiy.CWU = reportEntitiy.CWU.Where(tempQueryString);
                                break;
                        }
                        break;
                    case "string":
                        switch (entity)
                        {
                            case "Request":
                                reportEntitiy.Requests = reportEntitiy.Requests.Where(filterName[i] + ".StartsWith(@0)", Filters[filterCounter - 1][2]);
                                break;
                            case "Task":
                                reportEntitiy.Tasks = reportEntitiy.Tasks.Where(filterName[i] + ".StartsWith(@0)", Filters[filterCounter - 1][2]);
                                break;
                            case "SystemUser":
                                reportEntitiy.Users = reportEntitiy.Users.Where(filterName[i] + ".StartsWith(@0)", Filters[filterCounter - 1][2]);
                                break;
                            case "CallWrapUp":
                                reportEntitiy.CWU = reportEntitiy.CWU.Where(filterName[i] + ".StartsWith(@0)", Filters[filterCounter - 1][2]);
                                break;
                        }
                        break;
                    case "startdatetime":
                        switch (entity)
                        {
                            case "Request":
                                reportEntitiy.Requests = reportEntitiy.Requests.Where(filterName[i] + " > @0", DateTime.ParseExact(Filters[filterCounter-1][2], "dd/MM/yyyy", null));
                                break;
                            case "Task":
                                reportEntitiy.Tasks = reportEntitiy.Tasks.Where(filterName[i] + " > @0", DateTime.ParseExact(Filters[filterCounter-1][2], "dd/MM/yyyy", null));
                                break;
                            case "SystemUser":
                                reportEntitiy.Users = reportEntitiy.Users.Where(filterName[i] + " > @0", DateTime.ParseExact(Filters[filterCounter-1][2], "dd/MM/yyyy", null));
                                break;
                            case "CallWrapUp":
                                reportEntitiy.CWU = reportEntitiy.CWU.Where(filterName[i] + " > @0", DateTime.ParseExact(Filters[filterCounter-1][2], "dd/MM/yyyy", null));
                                break;
                        }
                        break;
                    case "enddatetime":
                        switch (entity)
                        {
                            case "Request":
                                reportEntitiy.Requests = reportEntitiy.Requests.Where(filterName[i] + " < @0", DateTime.ParseExact(Filters[filterCounter - 1][2], "dd/MM/yyyy", null).AddDays(1));
                                break;
                            case "Task":
                                reportEntitiy.Tasks = reportEntitiy.Tasks.Where(filterName[i] + " < @0", DateTime.ParseExact(Filters[filterCounter - 1][2], "dd/MM/yyyy", null).AddDays(1));
                                break;
                            case "SystemUser":
                                reportEntitiy.Users = reportEntitiy.Users.Where(filterName[i] + " < @0", DateTime.ParseExact(Filters[filterCounter - 1][2], "dd/MM/yyyy", null).AddDays(1));
                                break;
                            case "CallWrapUp":
                                reportEntitiy.CWU = reportEntitiy.CWU.Where(filterName[i] + " < @0", DateTime.ParseExact(Filters[filterCounter - 1][2], "dd/MM/yyyy", null).AddDays(1));
                                break;
                        }
                        break;
                }
            }

            List<string> fieldLabels = new List<string>();
            List<string> fieldNames = new List<string>();
            List<Guid> IDs = new List<Guid>();
            string strColumns = Parameters[Parameters.Length - 4];
            string[] arrColumn = strColumns.Split(',');

            if (!string.IsNullOrEmpty(strColumns))
            {
                for (int i = 0; i < arrColumn.Length; i++)
                {
                    int separatorIdx = arrColumn[i].IndexOf('=');
                    fieldLabels.Add(arrColumn[i].Substring(0, separatorIdx));
                    fieldNames.Add(arrColumn[i].Substring(separatorIdx + 1));
                }
            }

            DataTable data = new DataTable();
            for (int i = 0; i < fieldNames.Count; i++)
                data.Columns.Add(fieldNames[i]);

            bool hyperlinkFlag = false;
            if (entity == "Request" && Parameters[Parameters.Length - 2] == "true")
            {
                hyperlinkFlag = true;
                data.Columns.Add("URL");
            }

            DataRow dataRow = data.NewRow();

            var input = new List<dynamic>();

            switch (entity)
            {
                case "Request":
                    foreach (var x in reportEntitiy.Requests)
                    {
                        input.Add(x);
                    }
                    break;
                case "Task":
                    foreach (var x in reportEntitiy.Tasks)
                    {
                        input.Add(x);
                    }
                    break;
                case "SystemUser":
                    foreach (var x in reportEntitiy.Users)
                    {
                        input.Add(x);
                    }
                    break;
                case "CallWrapUp":
                    foreach (var x in reportEntitiy.CWU)
                    {
                        input.Add(x);
                    }
                    break;
            }

            foreach (var entry in input)
            {
                dataRow = data.NewRow();
                Guid ID = new Guid();
                switch (entity)
                {
                    case "Request":
                        ID = entry.RequestID;
                        break;
                    case "Task":
                        ID = entry.TaskID;
                        break;
                    case "SystemUser":
                        ID = entry.SystemUserID;
                        break;
                    case "CallWrapUp":
                        ID = entry.CallWrapUpID;
                        break;
                }

                string hrefURL = ConfigurationManager.AppSettings["AppServer"].ToString() + entity + "/Edit?id=" + ID;
                for (int j = 0; j < fieldNames.Count(); j++)
                {
                    if ("TicketNumber".Equals(fieldNames[j]))
                    {
                        string url = "<A href=\"" + hrefURL + "\">" + entry.GetType().GetProperty(fieldNames[j]).GetValue(entry) + "</A>";
                        dataRow[fieldNames[j]] = "<A href=\"" + hrefURL + "\">" + entry.GetType().GetProperty(fieldNames[j]).GetValue(entry) + "</A>";
                    }
                    else
                    {
                        dataRow[fieldNames[j]] = entry.GetType().GetProperty(fieldNames[j]).GetValue(entry);
                    }
                }

                if (hyperlinkFlag == true)
                    dataRow[fieldNames.Count] = hrefURL;

                data.Rows.Add(dataRow);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(data);

            string reportName = Parameters[Parameters.Length - 3];
            string[] arrReportName = reportName.Split('=');
            rpt.ReportTitle = arrReportName[2].Replace("%20", " ");
            rpt.ReportName = rpt.ReportTitle + " - " + DateTime.Now.ToString("ddMMyyyy - HH.mm.ss");
            rpt.DataBind(ds);
            
            rpt.Visible = true;
        }
    }
}