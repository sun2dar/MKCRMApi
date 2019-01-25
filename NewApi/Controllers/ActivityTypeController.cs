using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCARE.Models;
using CCARE.Service;
using CCARE.Repository;

namespace NewApi.Controllers
{
    public class ActivityTypeController : ApiController
    {
        ActivityTypeService service = new ActivityTypeService();
        ActivityTypeRepository repo = new ActivityTypeRepository();
        // GET api/<controller>
        public MyResult<List<ActivityType>> Get()
        {
            return service.findAll(repo);
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