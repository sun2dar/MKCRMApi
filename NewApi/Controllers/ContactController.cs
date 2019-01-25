using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCARE.Models.MasterData;
using CCARE.Repository;
using CCARE.Service;

namespace NewApi.Controllers
{
    public class ContactController : ApiController
    {
        ContactService service = new ContactService();
        ContactRepository repo = new ContactRepository();
        // GET: api/Lead
        public MyResult<List<Contact>> Get()
        {
            return service.findAll(repo);
            //return new string[] { "value1", "value2" };
        }

        // GET: api/Lead/5
        public MyResult<Contact> Get(Guid id)
        {
            return service.findById(repo, id);
        }

        // POST: api/Lead
        public MyResult<Contact> Post([FromBody]Contact model)
        {
            return service.create(repo, model);
        }

        // PUT: api/Lead/5
        public MyResult<Contact> Put(Guid id, [FromBody]Contact model)
        {
            return service.update(repo, model);
        }
    }
}