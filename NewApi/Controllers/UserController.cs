using CCARE.Models;
using CCARE.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCARE.Repository;

namespace NewApi.Controllers
{
    [Authorize]
    public class UserController : ApiController
    {
        UserService service = new UserService();
        UserRepository repo = new UserRepository();
        // GET: api/User
        public MyResult<List<SystemUser>> Get()
        {
            return service.findAll(repo);
        }

        // GET: api/User/5
        public MyResult<SystemUser> Get(Guid id)
        {
            return service.findById(repo, id);
        }

        // POST: api/User
        public MyResult<SystemUser> Post([FromBody]SystemUser model)
        {
            return service.create(repo, model);
        }

        // PUT: api/User/5
        public MyResult<SystemUser> Put(Guid id, [FromBody]SystemUser model)
        {
            return service.update(repo, model);
        }

        //// DELETE: api/User/5
        //public void Delete(int id)
        //{
        //}
    }
}
