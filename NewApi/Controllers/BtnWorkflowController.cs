using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCARE.Models.Btn;
using CCARE.Service;
using CCARE.Repository;

namespace NewApi.Controllers
{
    public class BtnWorkflowController : ApiController
    {
        BtnWorkflowService service = new BtnWorkflowService();
        BtnWorkflowRepository repo = new BtnWorkflowRepository();
        // GET api/<controller>
        public MyResult<List<BtnWorkflow>> Get()
        {
            return service.findAll(repo);
        }

        [ActionName("GetByProductTypeAndCurrentSeq")]
        public MyResult<List<BtnWorkflow>> Get(int ProductType, int Seq)
        {
            return service.findByProductTypeAndCurrentSeq(ProductType, Seq);
        }

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}