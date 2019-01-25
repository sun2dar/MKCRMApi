using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models;
using CCARE.Repository;
using CCARE.App_Function;

namespace CCARE.Service
{
    public class StringMapService : GenericService<StringMap>
    {
        StringMapRepository repo = new StringMapRepository();

        //public MyResult<StringMap> create(StringMap model)
        //{
            
        //}

        //public MyResult<StringMap> delete(Guid ID)
        //{
        //    throw new NotImplementedException();
        //}

        //public MyResult<StringMap> update(StringMap model)
        //{
        //    throw new NotImplementedException();
        //}

        public MyResult<List<StringMap>> getProductType()
        {
            try
            {
                return new MyResult<List<StringMap>>(0, "Sukses", repo.findByEntityAndAttributeName("Product", "BtnType"));
            }
            catch (Exception ex)
            {
                return new MyResult<List<StringMap>>(1, ex.Message, null);
            }
        }

        public MyResult<List<StringMap>> getActivityState()
        {
            try
            {
                return new MyResult<List<StringMap>>(0, "Sukses", repo.findByEntityAndAttributeName("activitypointer", "statecode"));
            }
            catch (Exception ex)
            {
                return new MyResult<List<StringMap>>(1, ex.Message, null);
            }
        }
    }
}