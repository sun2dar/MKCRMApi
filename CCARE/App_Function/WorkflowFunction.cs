using CCARE.Models.General;
using CCARE.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.App_Function
{
    public class WorkflowFunction
    {
        public static bool hasNext(Guid requestID)
        {
            CRMDb db = new CRMDb();
            Request_Workflow rw = new Request_Workflow();

            rw = db.Request_Workflow
                .Where(x => x.RequestID == requestID)
                .Where(x => x.Status == 0).FirstOrDefault();

            if (rw == null) return false;

            if (db.Workflow.Where(x => x.SLID == rw.ServiceLevelID).Any(x => x.SeqNo > rw.SeqNo))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool hasPrev(Guid requestID)
        {
            CRMDb db = new CRMDb();
            Request_Workflow rw = new Request_Workflow();

            rw = db.Request_Workflow
                .Where(x => x.RequestID == requestID)
                .Where(x => x.Status == 0).FirstOrDefault();

            if (rw == null) return false;

            if (db.Workflow.Where(x => x.SLID == rw.ServiceLevelID).Any(x => x.SeqNo < rw.SeqNo))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}