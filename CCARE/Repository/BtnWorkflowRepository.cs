using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models.Btn;
using CCARE.Models.General;

namespace CCARE.Repository
{
    public class BtnWorkflowRepository : MyRepository<BtnWorkflow>
    {
        CRMDb db = new CRMDb();
        public KeyValuePair<int, string> create(BtnWorkflow entity)
        {
            throw new NotImplementedException();
        }

        public List<BtnWorkflow> findAll()
        {
            return db.btnWorkflow.ToList();
        }

        public BtnWorkflow findById(Guid ID)
        {
            return db.btnWorkflow.Where(x => x.ID == ID).FirstOrDefault();
        }

        public List<BtnWorkflow> findPaging(int size, int page)
        {
            return db.btnWorkflow.Take(size).Skip(page).ToList();
        }

        public KeyValuePair<int, string> update(BtnWorkflow entity)
        {
            throw new NotImplementedException();
        }

        //Sequence Start from 1
        public List<BtnWorkflow> findByProductTypeAndCurrentSeq(int productType, int Seq)
        {
            return db.btnWorkflow
                .Where(x=> x.ProductType == productType && x.Seq > Seq)
                .OrderBy(x=> x.Seq)
                .ToList();
        }

        public List<BtnWorkflow> findByName(string name)
        {
            return db.btnWorkflow.Where(x => x.ProductTypeName.ToUpper().Contains(name.ToUpper())).ToList();
        }
    }
}