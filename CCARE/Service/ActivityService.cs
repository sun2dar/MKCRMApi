using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models;
using CCARE.App_Function;
using CCARE.Repository;


namespace CCARE.Service
{
    public class ActivityService : GenericService<Activities>
    {
        ActivitiesRepository repo = new ActivitiesRepository();

        public MyResult<List<Activities>> findByLeadId(Guid ID)
        {
            try
            {
                return new MyResult<List<Activities>>(0, "Sukses", repo.findByLeadId(ID));
            }
            catch (Exception ex)
            {
                return new MyResult<List<Activities>>(1, ex.Message, null);
            }
        }
        public MyResult<Activities> changeState(Guid ID, int statecode, Guid modifiedby)
        {
            KeyValuePair<int, string> results = repo.changeState(ID, statecode, modifiedby);
            int resultcode = Utility.CheckSpResult(results.Key);

            return new MyResult<Activities>(resultcode, results.Value, null);
        }

        //public MyResult<Activities> create(Activities model)
        //{
        //    if (model.ActivityID == null || model.ActivityID == Guid.Empty)
        //    {
        //        model.ActivityID = Guid.NewGuid();
        //    }

        //    KeyValuePair<int, string> results = repo.create(model);
        //    int resultcode = Utility.CheckSpResult(results.Key);

        //    return new MyResult<Activities>(resultcode, results.Value, null);
        //}

        //public MyResult<Activities> delete(Guid ID)
        //{
        //    throw new NotImplementedException();
        //}

        //public MyResult<List<Activities>> findAll()
        //{
        //    try
        //    {
        //        return new MyResult<List<Activities>>(0, "Sukses", repo.findAll());
        //    }
        //    catch (Exception ex)
        //    {
        //        return new MyResult<List<Activities>>(1, ex.Message, null);
        //    }
        //}

        //public MyResult<Activities> findById(Guid ID)
        //{
        //    try
        //    {
        //        return new MyResult<Activities>(0, "Sukses", repo.findById(ID));
        //    }
        //    catch (Exception ex)
        //    {
        //        return new MyResult<Activities>(1, ex.Message, null);
        //    }
        //}



        //public MyResult<List<Activities>> findByName(string name)
        //{
        //    throw new NotImplementedException();
        //}

        //public MyResult<Activities> update(Activities model)
        //{
        //    KeyValuePair<int, string> results = repo.update(model);
        //    int resultcode = Utility.CheckSpResult(results.Key);

        //    return new MyResult<Activities>(resultcode, results.Value, null);
        //}
    }
}