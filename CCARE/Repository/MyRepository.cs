using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Repository
{
    public interface MyRepository <T>
    {
        List<T> findAll();

        List<T> findPaging(int size, int page);

        List<T> findByName(String name);

        T findById(Guid ID);

        KeyValuePair<int, string> create(T entity);

        KeyValuePair<int, string> update(T entity);
    }
}