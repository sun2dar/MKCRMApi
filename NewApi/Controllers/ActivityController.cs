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
    public class ActivityController : ApiController
    {
        ActivitiesRepository repo = new ActivitiesRepository();
        ActivityService service = new ActivityService();
        // GET api/<controller>
        [HttpGet]
        [ActionName("Get")]
        public MyResult<List<Activities>> Get()
        {
            return service.findAll(repo);
        }

        // GET api/<controller>/5
        public MyResult<Activities> Get(Guid id)
        {
            return service.findById(repo,id);
        }

        [HttpGet]
        [ActionName("FindByLead")]
        public MyResult<List<Activities>> FindByLead(Guid id)
        {
            return service.findByLeadId(id);
        }

        //POST api/<controller>
        [HttpPost]
        [ActionName("Post")]
        public MyResult<Activities> Post([FromBody]Activities model)
        {
            return service.create(repo, model);
        }

        [HttpPost]
        [ActionName("ChangeState")]
        public MyResult<Activities> Post([FromBody]Guid ID, int statecode, Guid modifiedby)
        {
            return service.changeState(ID, statecode, modifiedby);
        }

        // PUT api/<controller>/5
        public MyResult<Activities> Put(Guid id, [FromBody]Activities model)
        {
            return service.update(repo, model);
        }

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}
    }
}