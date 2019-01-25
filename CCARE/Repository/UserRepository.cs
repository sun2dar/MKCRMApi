using CCARE.Models;
using CCARE.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Repository
{
    public class UserRepository : MyRepository<SystemUser>
    {
        CRMDb db = new CRMDb();

        public KeyValuePair<int, string> create(SystemUser entity)
        {
            return db.SystemUser_Insert(entity);
        }

        public List<SystemUser> findAll()
        {
            return db.systemUser.ToList();
        }

        public SystemUser authenticate(String username, String password)
        {
            return db.systemUser.Where(x=>x.DomainName == username).FirstOrDefault();
        }

        public SystemUser findById(Guid ID)
        {
            return db.systemUser.Where(x => x.SystemUserId == x.SystemUserId).First();
        }

        public List<SystemUser> findPaging(int size, int page)
        {
            return db.systemUser.Where(x=>x.DeletionStateCode == 0).Take(size).Skip(page).ToList();
        }

        public KeyValuePair<int, string> update(SystemUser entity)
        {
            return db.SystemUser_Update(entity);
        }

        public List<SystemUser> findByName(string name)
        {
            return db.systemUser.Where(x=> x.DeletionStateCode == 0 && x.FullName.ToUpper().Contains(name.ToUpper())).ToList();
        }
    }
}