using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models.General;
using CCARE.Models;

namespace CCARE.Repository
{
    public class ProductRepository : MyRepository<Product>
    {
        CRMDb db = new CRMDb();
        public KeyValuePair<int, string> create(Product entity)
        {
            return db.Product_Insert_Btn(entity);
        }

        public List<Product> findAll()
        {
            return db.product.Where(x => x.DeletionStateCode == 0 && x.ProductTypeCode != null).ToList();
        }

        public Product findById(Guid ID)
        {
            return db.product.Where(x => x.DeletionStateCode == 0 && x.ProductTypeCode != null && x.ProductID == ID).FirstOrDefault();
        }

        public List<Product> findByName(string name)
        {
            return db.product.Where(x => x.DeletionStateCode == 0 && x.Name.ToUpper().Contains(name.ToUpper())).ToList();
        }

        public List<Product> findPaging(int size, int page)
        {
            return db.product.Where(x => x.DeletionStateCode == 0 && x.ProductTypeCode != null).Take(size).Skip(page).ToList(); 
        }

        public KeyValuePair<int, string> update(Product entity)
        {
            return db.Product_Update_Btn(entity);
        }
    }
}