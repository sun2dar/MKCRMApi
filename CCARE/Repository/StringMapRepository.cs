using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models;
using CCARE.Models.General;

namespace CCARE.Repository
{
    public class StringMapRepository : MyRepository<StringMap>
    {
        CRMDb db = new CRMDb();
        public KeyValuePair<int, string> create(StringMap entity)
        {
            throw new NotImplementedException();
        }

        public List<StringMap> findAll()
        {
            return db.pickList.ToList();
        }

        public StringMap findById(Guid ID)
        {
            return db.pickList.Where(x => x.StringMapID == ID).FirstOrDefault();
        }

        public List<StringMap> findPaging(int size, int page)
        {
            return db.pickList.Take(size).Skip(page).ToList();
        }

        public KeyValuePair<int, string> update(StringMap entity)
        {
            throw new NotImplementedException();
        }

        public List<StringMap> findByEntityAndAttributeName(String entityName, String attributename)
        {
            return db.pickList
                .Where(x=> x.EntityName == entityName && x.AttributeName == attributename)
                .OrderBy(x=> x.AttributeValue)
                .ToList();
        }

        public List<StringMap> findByName(string name)
        {
            return db.pickList
                .Where(x => x.EntityName.Contains(name) && x.AttributeName.Contains(name))
                .OrderBy(x => x.AttributeValue)
                .ToList();
        }
    }
}