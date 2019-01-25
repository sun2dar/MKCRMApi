using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models.SalesMarketing;
using CCARE.Repository;

namespace NewApi.Service
{
    public class LeadService : MyService<Lead>
    {
        LeadRepository repo = new LeadRepository();

        public MyResult<Lead> create(Lead model)
        {
            int resultcode = 1;

            if (model.LeadID == null || model.LeadID == Guid.Empty)
            {
                model.LeadID = Guid.NewGuid();
            }

            KeyValuePair<int, string> results = repo.create(model);
            if (results.Key == 0 || results.Key == 16 || results.Key == 6)
            {
                resultcode = 0;
            }

            return new MyResult<Lead>(resultcode, results.Value, null);
        }

        public MyResult<Lead> delete(Guid ID)
        {
            throw new NotImplementedException();
        }

        public MyResult<List<Lead>> findAll()
        {
            try
            {
                return new MyResult<List<Lead>>(0, "Sukses", repo.findAll());
            }
            catch (Exception ex)
            {
                return new MyResult<List<Lead>>(1, ex.StackTrace, null);
            }
        }

        public MyResult<Lead> findById(Guid ID)
        {
            throw new NotImplementedException();
        }

        public MyResult<Lead> findByName(string name)
        {
            throw new NotImplementedException();
        }

        public MyResult<Lead> update(Lead model)
        {
            throw new NotImplementedException();
        }
    }
}