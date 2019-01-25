using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Repository;

namespace NewApi.Service
{
    public interface MyService<T>
    {
        MyResult<List<T>> findAll();
        MyResult<T> findById(Guid ID);
        MyResult<T> findByName(string name);
        MyResult<T> create(T model);
        MyResult<T> update(T model);
        MyResult<T> delete(Guid ID);
    }

    //public class TestService<T, U> : MyRepositor
    //{
    //    public MyResult<List<T>> findAll()
    //    {
    //        MyRepository<T> repo = new MyRepository<T>();
    //        try
    //        {
    //            return new MyResult<List<T>>(0, "Sukses", repo.findAll());
    //        }
    //        catch (Exception ex)
    //        {
    //            return new MyResult<List<T>>(1, ex.StackTrace, null);
    //        }
    //    }
    //}
}