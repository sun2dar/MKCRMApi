using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CCARE.Models;
using CCARE.Repository;
using CCARE.Service;

namespace NewApi.Controllers
{
    public class ProductController : ApiController
    {
        ProductService service = new ProductService();
        ProductRepository repo = new ProductRepository();
        // GET: api/Lead
        public MyResult<List<Product>> Get()
        {
            return service.findAll(repo);
            //return new string[] { "value1", "value2" };
        }

        // GET: api/Lead/5
        public MyResult<Product> Get(Guid id)
        {
            return service.findById(repo, id);
        }

        // POST: api/Lead
        public MyResult<Product> Post([FromBody]Product model)
        {
            return service.create(repo, model);
        }

        // PUT: api/Lead/5
        public MyResult<Product> Put(Guid id, [FromBody]Product model)
        {
            return service.update(repo, model);
        }
    }
}