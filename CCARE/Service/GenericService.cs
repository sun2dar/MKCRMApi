using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Repository;
using CCARE.App_Function;

namespace CCARE.Service
{
    public abstract class GenericService<T>
    {
        public virtual MyResult<List<T>> findAll(MyRepository<T> repo)
        {
            try
            {
                return new MyResult<List<T>>(0, "Sukses", repo.findAll());
            }
            catch (Exception ex)
            {
                return new MyResult<List<T>>(1, ex.Message, null);
            }
        }

        public virtual MyResult<T> findById(MyRepository<T> repo, Guid ID)
        {
            try
            {
                return new MyResult<T>(0, "Sukses", repo.findById(ID));
            }
            catch (Exception ex)
            {
                return new MyResult<T>(1, ex.Message, default(T));
            }
        }

        public virtual MyResult<List<T>> findByName(MyRepository<T> repo, string name)
        {
            try
            {
                return new MyResult<List<T>>(0, "Sukses", repo.findByName(name));
            }
            catch (Exception ex)
            {
                return new MyResult<List<T>>(1, ex.Message, null);
            }
        }

        public virtual MyResult<T> create(MyRepository<T> repo, T model)
        {
            KeyValuePair<int, string> results = repo.create(model);
            int resultcode = Utility.CheckSpResult(results.Key);

            return new MyResult<T>(resultcode, results.Value, default(T));
        }

        public virtual MyResult<T> delete(MyRepository<T> repo, Guid ID)
        {
            throw new NotImplementedException();
        }

        public virtual MyResult<T> update(MyRepository<T> repo, T model)
        {
            KeyValuePair<int, string> results = repo.update(model);
            int resultcode = Utility.CheckSpResult(results.Key);

            return new MyResult<T>(resultcode, results.Value, default(T));
        }

        //public abstract MyResult<T> create(T model);
        //public abstract MyResult<T> update(T model);
        //public abstract MyResult<T> delete(Guid ID);
    }
}