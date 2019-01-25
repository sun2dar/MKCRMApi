using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCARE.Service;
using CCARE.Models;
using CCARE.Repository;

namespace NewApi.Controllers
{
    //[Authorize]
    //[Route("api/[controller]/[action]")]
    public class StringMapController : ApiController
    {
        //GenericService<StringMap> service = new StringMapService();
        StringMapService myservice = new StringMapService();
        StringMapRepository repo = new StringMapRepository();

        // GET api/<controller>
        //[ActionName("Get")]
        public MyResult<List<StringMap>> Get() {
            return myservice.findAll(repo);
        }

        [ActionName("GetProductType")]
        public MyResult<List<StringMap>> GetProductType()
        {
            return myservice.getProductType();
        }

        [ActionName("GetActivityState")]
        public MyResult<List<StringMap>> GetActivityState()
        {
            return myservice.getActivityState();
        }

        //// GET api/<controller>/5
        //public string Get(int id)
        //{
        //    //service.PrintHello();
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