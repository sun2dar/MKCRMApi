using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models.Btn;
using CCARE.Repository;
using CCARE.App_Function;

namespace CCARE.Service
{
    public class BtnWorkflowService : GenericService<BtnWorkflow>
    {
        BtnWorkflowRepository repo = new BtnWorkflowRepository();

        public MyResult<List<BtnWorkflow>> findByProductTypeAndCurrentSeq(int productType, int seq)
        {
            try
            {
                return new MyResult<List<BtnWorkflow>>(0, "Sukses", repo.findByProductTypeAndCurrentSeq(productType, seq));
            }
            catch (Exception ex)
            {
                return new MyResult<List<BtnWorkflow>>(1, ex.Message, null);
            }
        }

        //public MyResult<BtnWorkflow> create(BtnWorkflow model)
        //{
        //    throw new NotImplementedException();
        //}

        //public MyResult<BtnWorkflow> delete(Guid ID)
        //{
        //    throw new NotImplementedException();
        //}

        //public MyResult<List<BtnWorkflow>> findAll()
        //{
        //    try
        //    {
        //        return new MyResult<List<BtnWorkflow>>(0, "Sukses", repo.findAll());
        //    }
        //    catch (Exception ex)
        //    {
        //        return new MyResult<List<BtnWorkflow>>(1, ex.Message, null);
        //    }
        //}


        //public MyResult<BtnWorkflow> findById(Guid ID)
        //{
        //    try
        //    {
        //        return new MyResult<BtnWorkflow>(0, "Sukses", repo.findById(ID));
        //    }
        //    catch (Exception ex)
        //    {
        //        return new MyResult<BtnWorkflow>(1, ex.Message, null);
        //    }
        //}

        //public MyResult<List<BtnWorkflow>> findByName(string name)
        //{
        //    throw new NotImplementedException();
        //}

        //public MyResult<BtnWorkflow> update(BtnWorkflow model)
        //{
        //    throw new NotImplementedException();
        //}
    }
}