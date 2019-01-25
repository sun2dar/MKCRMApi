using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models;
using CCARE.Models.General;

namespace CCARE.Repository
{
    public class QueueItemRepository : MyRepository<QueueItem>
    {
        CRMDb db = new CRMDb();
        public KeyValuePair<int, string> create(QueueItem entity)
        {
            throw new NotImplementedException();
        }

        public List<QueueItem> findAll()
        {
            return db.queueitem.Where(x => x.DeletionStateCode == 0 && x.ObjectTypeCode == 4).ToList();
        }

        public QueueItem findById(Guid ID)
        {
            return db.queueitem.Where(x => x.QueueItemId == ID).FirstOrDefault();
        }

        public List<QueueItem> findByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<QueueItem> findPaging(int size, int page)
        {
            return db.queueitem.Where(x => x.DeletionStateCode == 0 && x.ObjectTypeCode == 4).Take(size).Skip(page).ToList();
        }

        public KeyValuePair<int, string> update(QueueItem entity)
        {
            throw new NotImplementedException();
        }
    }
}