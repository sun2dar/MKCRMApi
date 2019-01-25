using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models;
using CCARE.Models.General;
using CCARE.Models.SalesMarketing;

namespace CCARE.Repository
{
    public class LeadRepository : MyRepository<Lead>
    {
        CRMDb db = new CRMDb();
        public List<Lead> findAll()
        {
            return db.lead.Where(x => x.DeletionStateCode == 0).ToList();
        }

        public Lead findById(Guid ID)
        {
            return db.lead.Where(x => x.LeadID == ID).FirstOrDefault();
        }

        public List<Lead> findByName(string name)
        {
            return db.lead.Where(x => x.Fullname.Contains(name)).ToList();
        }

        public KeyValuePair<int, string> create(Lead lead)
        {
            return db.Lead_Insert(lead);
        }

        public KeyValuePair<int, string> update(Lead lead)
        {
            return db.Lead_Update(lead);
        }

        public List<Lead> findPaging(int size, int page)
        {
            return db.lead.Where(x => x.DeletionStateCode == 0).Take(size).Skip(page).ToList();
        }

        public KeyValuePair<int, string> changeStatus(Guid ID, int StatusCode, Guid ModifiedBy)
        {
            return db.Lead_ChangeStatus(ID, StatusCode, ModifiedBy);
        }
    }
}