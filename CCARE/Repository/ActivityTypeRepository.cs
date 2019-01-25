using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models.General;
using CCARE.Models;

namespace CCARE.Repository
{
    public class ActivityTypeRepository : MyRepository<ActivityType>
    {
        CRMDb db = new CRMDb();
        public KeyValuePair<int, string> create(ActivityType entity)
        {
            throw new NotImplementedException();
        }

        public List<ActivityType> findAll()
        {
            return db.activityType
                .OrderBy(x=> x.DisplayOrder)
                .ToList();
        }

        public ActivityType findById(Guid ID)
        {
            return db.activityType.Where(x => x.StringMapID == ID).FirstOrDefault();
        }

        public List<ActivityType> findByName(string name)
        {
            return db.activityType.Where(x => x.EntityName.ToUpper().Contains(name.ToUpper()) && x.AttributeName.ToUpper().Contains(name.ToUpper())).ToList();
        }

        public List<ActivityType> findPaging(int size, int page)
        {
            return db.activityType
                .OrderBy(x => x.DisplayOrder)
                .Take(size).Skip(page).ToList();
        }

        public KeyValuePair<int, string> update(ActivityType entity)
        {
            throw new NotImplementedException();
        }
    }
}