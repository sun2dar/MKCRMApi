using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCARE.Models.SalesMarketing;
using CCARE.Repository;
using CCARE.Service;

namespace NewApi.Controllers
{
    [Authorize]
    public class LeadController : ApiController
    {
        LeadService service = new LeadService();
        LeadRepository repo = new LeadRepository();
        // GET: api/Lead
        public MyResult<List<Lead>> Get()
        {
            return service.findAll(repo);
            //return new string[] { "value1", "value2" };
        }

        // GET: api/Lead/5
        public MyResult<Lead> Get(Guid id)
        {
            return service.findById(repo, id);
        }

        // POST: api/Lead
        public MyResult<Lead> Post([FromBody]Lead model)
        {
            return service.create(repo, model);
        }

        [HttpPost]
        [ActionName("ChangeStatus")]
        public MyResult<Lead> ChangeStatus([FromBody]Guid ID, int statusCode, Guid modifiedby)
        {
            return service.changeStatus(ID, statusCode, modifiedby);
        }

        // PUT: api/Lead/5
        public MyResult<Lead> Put(Guid id, [FromBody]Lead model)
        {
            return service.update(repo, model);
        }

        //// DELETE: api/Lead/5
        //public void Delete(int id)
        //{
        //}
    }
}
