//Created by Dwi Marti A R
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models.General;
using System.Data.Objects;
using System.Collections.Specialized;

namespace CCARE.Models
{
	public class StatusLabel
	{
        public string Value { get; set; }
        public string Text { get; set; }

        public static List<StatusLabel> GetLabel(ChangeStatusInput input)
        {
            CRMDb db = new CRMDb();

            try
            {
                ObjectResult<StatusLabel> newLabelList = db.GetStatusName(input);
                
                var labelList = (from x in newLabelList.AsEnumerable()
                                 select new StatusLabel
                                 {
                                     Value = x.Value,
                                     Text = x.Text
                                 }).ToList();

                return labelList;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<StatusLabel> GetNextStatus(ChangeStatusInput input)
        {
            CRMDb db = new CRMDb();
            
            try
            {
                ObjectResult<StatusLabel> newStatusList = db.GetNewStatus(input);
                
                var statusList = (from x in newStatusList
                                  select new StatusLabel
                                  {
                                      Value = x.Value,
                                      Text = x.Text
                                  }).OrderBy(x => x.Value).ToList();
                
                return statusList;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<StatusLabel> GetChangeStatusReason(int? entityType)
        {
            CRMDb db = new CRMDb();

            try
            {
                var reason = (from x in db.statusmapper
                              where x.EntityType == entityType
                              where x.StatusType == 99
                              select new StatusLabel
                              {
                                 Value = x.Key,
                                 Text = x.Value
                              }).OrderBy(x => x.Value).ToList();
                return reason;

            }
            catch (Exception e)
            {
                return null;
            }
        }

    }

    public class ChangeStatusInput
    {
        public int? EntityType { get; set; }
        public int? StatusType { get; set; }
        public string Status { get; set; }
        public Guid? SecurityRoleId { get; set; }
    }

}