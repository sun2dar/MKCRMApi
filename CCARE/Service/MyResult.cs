using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.Service
{
    public class MyResult<T>
    {
        public int code { get; set; }
        public string message { get; set; }
        public T model { get; set; }

        public MyResult()
        {
        }

        public MyResult(int code, string message, T model)
        {
            this.code = code;
            this.message = message;
            this.model = model;
        }
    }

    //public class MyResultAlt<T, U>
    //{
    //    public int code { get; set; }
    //    public string message { get; set; }
    //    public T model { get; set; }
    //    public U repo { get; set; }

    //    public MyResultAlt() { }

    //    public MyResultAlt(int code, string message, T model, U repo)
    //    {
    //        this.code = code;
    //        this.message = message;
    //        this.model = model;
    //        this.repo = repo;
    //    }
    //}
}