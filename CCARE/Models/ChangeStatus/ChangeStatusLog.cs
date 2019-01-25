using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NLog;
using System.Text;

namespace CCARE.Models
{
    public class ChangeStatusLog
    {
        private static Logger changestatuslog = NLog.LogManager.GetLogger("ChangeStatusLog");

        public static void Start(Params param)
        {
            StringBuilder str = new StringBuilder();
            str
                .AppendFormat("Start Execute:\t{0} on {1}", param.RequestTransType, DateTime.Now.ToString())
                .AppendLine()
                .AppendFormat("Parameter(s):");
            for (int i = 0; i < param.Parameter.Count(); i++)
            {
                str.AppendFormat(" - {0}: {1}", param.Parameter.ElementAt(i).Key, param.Parameter.ElementAt(i).Value);
            }
            str
                .AppendLine()
                .AppendLine()
                .AppendLine("------------------------------------------------------" + "\r\n");
            changestatuslog.Debug(str);
        }

        public static void Finish(string requesttranstype, ChangeStatusResult result)
        {
            StringBuilder str = new StringBuilder();
            str
                .AppendFormat("Finish Execute:\t{0} on {1}", requesttranstype, DateTime.Now.ToString())
                .AppendLine()
                .AppendFormat("Output(s):")
                .AppendLine()
                .AppendFormat("Update Time:\t{0}", result.UpdateTime)
                .AppendLine()
                .AppendFormat("Updated By:\t{0}", result.UpdatedBy.Name)
                .AppendLine()
                .AppendFormat("Result:\t{0}", result.Result)
                .AppendLine()
                .AppendLine()
                .AppendLine("------------------------------------------------------" + "\r\n");
            changestatuslog.Debug(str);
        }

        public static void Write(Params param, ChangeStatusResult result, string start, string stop)
        {
            StringBuilder str = new StringBuilder();
            if (param != null)
            {
                str
                    .AppendFormat("{0}", param.RequestTransType)
                    .AppendLine()
                    .AppendFormat("Start Execute on {0}", start)
                    .AppendLine()
                    .AppendFormat("Finish Execute on {0}", stop)
                    .AppendLine()
                    .AppendFormat("Parameter(s):");
                for (int i = 0; i < param.Parameter.Count(); i++)
                {
                    str.AppendFormat("  - {0}: {1}", param.Parameter.ElementAt(i).Key, param.Parameter.ElementAt(i).Value);
                }
            }
            str
                .AppendLine()
                .AppendFormat("Output:")
                .AppendLine()
                .AppendFormat("\tUpdate Time:\t{0}", result.UpdateTime)
                .AppendLine()
                .AppendFormat("\tUpdated By:\t{0}", result.UpdatedBy.Name)
                .AppendLine()
                .AppendFormat("\tResult:\t{0}", result.Result)
                .AppendLine()
                .AppendFormat("\tStatus:\t{0}", result.Status)
                .AppendLine()
                .AppendLine()
                .AppendLine("------------------------------------------------------" + "\r\n");
            changestatuslog.Debug(str);
        }

        public static void Message(string entityType, string statusType, string oldval, string newval, string message)
        {
            StringBuilder str = new StringBuilder();
            str
                .AppendFormat("Entity: {0}", entityType)
                .AppendLine()
                .AppendFormat("Status: {0}", statusType)
                .AppendLine()
                .AppendFormat("Old value: {0} -> New value: {1}", oldval, newval)
                .AppendLine()
                .AppendFormat("Message:")
                .AppendLine()
                .AppendFormat("\t{0}", message)
                .AppendLine()
                .AppendLine()
                .AppendLine("------------------------------------------------------" + "\r\n");
            changestatuslog.Debug(str);
        }

    }
}