using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCARE.Repository;
using CCARE.Models.SalesMarketing;

namespace CCareApi.Controllers
{
    public class LeadController : ApiController
    {
        // GET api/lead
        LeadRepository leadRepo = new LeadRepository();

        public List<Lead> Get()
        {
            return leadRepo.findAll();
        }

        // GET api/lead/5
        public Lead Get(Guid id)
        {
            return leadRepo.findById(id);
        }

        // POST api/lead
        public KeyValuePair<int, String> Post([FromBody]Lead lead)
        {
            return leadRepo.create(lead);
        }

        // PUT api/lead/5
        public KeyValuePair<int, String> Put(Guid id, [FromBody]Lead lead)
        {
            return leadRepo.update(lead);
        }

        //// DELETE api/lead/5
        //public void Delete(int id)
        //{
        //}
    }
}
