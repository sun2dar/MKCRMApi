using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using CCARE.Models;
using CCARE.Repository;
using NewApi.App_Function;

namespace NewApi.Service
{
    public class UserService:MyService<SystemUser>
    {
        UserRepository userRepo = new UserRepository();

        public MyResult<SystemUser> authenticate(string username, string password)
        {
            String domainName = ConfigurationManager.AppSettings["DomainName_dev"];

            if(username == null || username == "")
            {
                return new MyResult<SystemUser>(0, "Username harus diisi", null);
            }
            else if(password == null || password == "")
            {
                return new MyResult<SystemUser>(0, "Password harus diisi", null);
            }
            else
            {
                if(DomainChecker.isDomainFormat(domainName, username) == false)
                {
                    username = domainName + "\\" + username;
                }

                //Repository and Model class in CCARE Project
                SystemUser user = userRepo.authenticate(username, password);

                if(user == null)
                {
                    return new MyResult<SystemUser>(0, "Username atau Password tidak valid", null);
                }
                else
                {
                    return new MyResult<SystemUser>(1, "Login sukses", user);
                }
            }
        }

        public MyResult<SystemUser> create(SystemUser model)
        {
            throw new NotImplementedException();
        }

        public MyResult<SystemUser> delete(Guid ID)
        {
            throw new NotImplementedException();
        }

        public MyResult<List<SystemUser>> findAll()
        {
            try
            {
                return new MyResult<List<SystemUser>>(0, "Sukses", userRepo.findAll());
            }
            catch (Exception ex)
            {
                return new MyResult<List<SystemUser>>(1, ex.StackTrace, null);
            }
        }

        public MyResult<SystemUser> findById(Guid ID)
        {
            try
            {
                return new MyResult<SystemUser>(0, "Sukses", userRepo.findById(ID));
            }
            catch (Exception ex)
            {
                return new MyResult<SystemUser>(1, ex.StackTrace, null);
            }
        }

        public MyResult<SystemUser> findByName(string name)
        {
            throw new NotImplementedException();
        }

        public MyResult<SystemUser> update(SystemUser model)
        {
            throw new NotImplementedException();
        }
    }
}