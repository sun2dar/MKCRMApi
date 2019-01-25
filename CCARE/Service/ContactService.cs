using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCARE.Models.MasterData;
using CCARE.Repository;
using CCARE.App_Function;

namespace CCARE.Service
{
    public class ContactService : GenericService<Contact>
    {
        //ContactRepository repo = new ContactRepository();
        //public MyResult<Contact> create(Contact model)
        //{
        //    if (model.ContactID == null || model.ContactID == Guid.Empty)
        //    {
        //        model.ContactID = Guid.NewGuid();
        //    }

        //    KeyValuePair<int, string> results = repo.create(model);
        //    int resultcode = Utility.CheckSpResult(results.Key);

        //    return new MyResult<Contact>(resultcode, results.Value, null);
        //}

        //public MyResult<Contact> delete(Guid ID)
        //{
        //    throw new NotImplementedException();
        //}

        //public MyResult<List<Contact>> findAll()
        //{
        //    try
        //    {
        //        return new MyResult<List<Contact>>(0, "Sukses", repo.findAll());
        //    }
        //    catch (Exception ex)
        //    {
        //        return new MyResult<List<Contact>>(1, ex.Message, null);
        //    }
        //}

        //public MyResult<Contact> findById(Guid ID)
        //{
        //    try
        //    {
        //        return new MyResult<Contact>(0, "Sukses", repo.findById(ID));
        //    }
        //    catch (Exception ex)
        //    {
        //        return new MyResult<Contact>(1, ex.Message, null);
        //    }
        //}

        //public MyResult<List<Contact>> findByName(string name)
        //{
        //    try
        //    {
        //        return new MyResult<List<Contact>>(0, "Sukses", repo.findByName(name));
        //    }
        //    catch (Exception ex)
        //    {
        //        return new MyResult<List<Contact>>(1, ex.Message, null);
        //    }
        //}

        //public MyResult<Contact> update(Contact model)
        //{
        //    if (model.ContactID == null || model.ContactID == Guid.Empty)
        //    {
        //        model.ContactID = Guid.NewGuid();
        //    }

        //    KeyValuePair<int, string> results = repo.update(model);
        //    int resultcode = Utility.CheckSpResult(results.Key);

        //    return new MyResult<Contact>(resultcode, results.Value, null);
        //}
    }
}