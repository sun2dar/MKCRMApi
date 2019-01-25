using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models.SalesMarketing;
using CCARE.Repository;
using CCARE.App_Function;

namespace CCARE.Service
{
    public class LeadService : GenericService<Lead>
    {
        LeadRepository repo = new LeadRepository();


        public MyResult<Lead> changeStatus(Guid ID, int StatusCode, Guid ModifiedBy)
        {
            KeyValuePair<int, string> results = repo.changeStatus(ID, StatusCode, ModifiedBy);
            int resultcode = Utility.CheckSpResult(results.Key);

            return new MyResult<Lead>(resultcode, results.Value, null);
        }

        //public MyResult<Lead> create(Lead model)
        //{
        //    if (model.LeadID == null || model.LeadID == Guid.Empty)
        //    {
        //        model.LeadID = Guid.NewGuid();
        //    }

        //    KeyValuePair<int, string> results = repo.create(model);
        //    int resultcode = Utility.CheckSpResult(results.Key);

        //    return new MyResult<Lead>(resultcode, results.Value, null);
        //}

        //public MyResult<Lead> delete(Guid ID)
        //{
        //    throw new NotImplementedException();
        //}

        //public MyResult<Lead> update(Lead model)
        //{
        //    KeyValuePair<int, string> results = repo.update(model);
        //    int resultcode = Utility.CheckSpResult(results.Key);

        //    return new MyResult<Lead>(resultcode, results.Value, null);
        //}

        //public MyResult<List<Lead>> findAll()
        //{
        //    try
        //    {
        //        return new MyResult<List<Lead>>(0, "Sukses", repo.findAll());
        //    }
        //    catch (Exception ex)
        //    {
        //        return new MyResult<List<Lead>>(1, ex.Message, null);
        //    }
        //}

        //public MyResult<Lead> findById(Guid ID)
        //{
        //    try
        //    {
        //        return new MyResult<Lead>(0, "Sukses", repo.findById(ID));
        //    }
        //    catch (Exception ex)
        //    {
        //        return new MyResult<Lead>(1, ex.Message, null);
        //    }
        //}

        //public MyResult<List<Lead>> findByName(string name)
        //{
        //    try
        //    {
        //        return new MyResult<List<Lead>>(0, "Sukses", repo.findByName(name));
        //    }
        //    catch (Exception ex)
        //    {
        //        return new MyResult<List<Lead>>(1, ex.Message, null);
        //    }
        //}

    }
}