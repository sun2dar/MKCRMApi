using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Models.General;
using System.Globalization;
using CCARE.jqGrid;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using System.Configuration;


namespace CCARE.Controllers
{
    public partial class RequestController : Controller
    {
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LoadRequest_Workflow(string sidx, string sord, int rows, int page = 1)
        {
            string getRequestID = Request["_requestID"];

            var data = db.Request_Workflow.Where(x => x.RequestID == new Guid(getRequestID))
                .OrderBy(x => x.AssignedDate).ToList()
                .Select(x => new
                {
                    Id = x.ID,
                    WorkgroupName = x.WorkgroupName,
                    SLADays = x.SLADays,
                    SeqNo = x.SeqNo,
                    isOverdue = x.IsOverdue,
                    ReopenNo = x.ReopenNo,
                    Status = x.Status,
                    AssignedDate = x.AssignedDate,
                    HandledDate = x.HandledDate,
                    RemainingSL = x.RemainingSL
                });

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = data.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            data = data.Skip((page - 1) * pageSize).Take(pageSize);

            var recordCount = data.Count();
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }

        //Load Workflow
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult LoadWorkflow(string sidx, string sord, int rows, int page = 1)
        {
            string getRequestID = Request["_requestID"];

            Guid SLID = Guid.NewGuid();
            int SeqNo = 0;

            var rw = db.Request_Workflow
                .Where(x => x.RequestID == new Guid(getRequestID))
                .Where(x => x.Status == 0).FirstOrDefault();

            if (rw != null)
            {
                SLID = new Guid(rw.ServiceLevelID.ToString()); ;
                SeqNo = (int)rw.SeqNo;
            }

            var data = db.Workflow
                .Where(x => x.SLID == SLID)
                .Where(x => x.SeqNo > SeqNo).ToList()
                .Select(x => new
                {
                    Id = x.ID,
                    WorkgroupName = x.WgName,
                    SLADays = x.WorkflowSLADays,
                    SeqNo = x.SeqNo,
                    Keterangan = x.Keterangan
                });

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            int totalRecords = data.Count();
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            data = data.Skip((page - 1) * pageSize).Take(pageSize);

            var recordCount = data.Count();
            JSONTable jTable = new JSONTable();
            jTable.total = totalPages;
            jTable.page = page;
            jTable.records = totalRecords;
            jTable.rows = data.ToArray();

            return Json(jTable, JsonRequestBehavior.AllowGet);
        }
    }
}
