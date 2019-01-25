using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models;
using CCARE.Repository;
using CCARE.App_Function;

namespace CCARE.Service
{
    public class ActivityTypeService : GenericService<ActivityType>
    {
        ActivityTypeRepository repo = new ActivityTypeRepository();
        //public MyResult<ActivityType> create(ActivityType model)
        //{
        //    throw new NotImplementedException();
        //}

        //public MyResult<ActivityType> delete(Guid ID)
        //{
        //    throw new NotImplementedException();
        //}

        //public MyResult<List<ActivityType>> findAll()
        //{
        //    try
        //    {
        //        return new MyResult<List<ActivityType>>(0, "Sukses", repo.findAll());
        //    }
        //    catch (Exception ex)
        //    {
        //        return new MyResult<List<ActivityType>>(1, ex.Message, null);
        //    }
        //}

        //public MyResult<ActivityType> findById(Guid ID)
        //{
        //    throw new NotImplementedException();
        //}

        //public MyResult<List<ActivityType>> findByName(string name)
        //{
        //    throw new NotImplementedException();
        //}

        //public MyResult<ActivityType> update(ActivityType model)
        //{
        //    throw new NotImplementedException();
        //}
    }
}