using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models;
using CCARE.Models.General;

namespace CCARE.Repository
{
    public class ActivitiesRepository : MyRepository<Activities>
    {
        CRMDb db = new CRMDb();
        public KeyValuePair<int, string> create(Activities entity)
        {
            return db.Activity_Insert(entity);
        }

        public List<Activities> findAll()
        {
            //Limited to Lead activity only
            return db.activity.Where(x => x.DeletionStateCode == 0 && x.ObjectTypeCode == 4).ToList();
        }

        public Activities findById(Guid ID)
        {
            return db.activity.Where(x => x.DeletionStateCode == 0 && x.ActivityID == ID).FirstOrDefault();
        }

        public List<Activities> findPaging(int size, int page)
        {
            return db.activity.Where(x => x.DeletionStateCode == 0 && x.ObjectTypeCode == 4).Take(size).Skip(page).ToList();
        }

        public KeyValuePair<int, string> update(Activities entity)
        {
            return db.Activity_Update(entity);
        }

        public KeyValuePair<int, string> changeState(Guid ID, int statecode, Guid modifiedby)
        {
            return db.Activity_ChangeState(ID, statecode, modifiedby);
        }

        public List<Activities> findByLeadId(Guid ID)
        {
            //Limited to Lead activity only
            return db.activity.Where(x => x.DeletionStateCode == 0 && x.RegardingObjectID == ID).ToList();
        }

        public List<Activities> findByName(string name)
        {
            return db.activity.Where(x => x.DeletionStateCode == 0 && x.Description.Contains(name)).ToList();
        }
    }
}