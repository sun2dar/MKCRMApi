using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models.MasterData;
using CCARE.Models.General;

namespace CCARE.Repository
{
    public class ContactRepository : MyRepository<Contact>
    {
        CRMDb db = new CRMDb();
        public KeyValuePair<int, string> create(Contact entity)
        {
            return db.Contact_Insert(entity);
        }

        public List<Contact> findAll()
        {
            return db.contact.Where(x => x.DeletionStateCode == 0).ToList();
        }

        public Contact findById(Guid ID)
        {
            return db.contact.Where(x => x.ContactID == ID).FirstOrDefault();
        }

        public List<Contact> findPaging(int size, int page)
        {
            return db.contact.Where(x => x.DeletionStateCode == 0).Take(size).Skip(page).ToList();
        }

        public KeyValuePair<int, string> update(Contact entity)
        {
            return db.Contact_Update(entity);
        }

        public List<Contact> findByName(string name)
        {
            return db.contact.Where(x => x.FullName.ToUpper().Contains(name.ToUpper())).ToList();
        }
    }
}