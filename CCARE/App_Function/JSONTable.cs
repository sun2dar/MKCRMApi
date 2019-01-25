using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCARE.App_Function
{
    public struct JSONTable
    {
        private int _page;
        private decimal _total;
        private int _records;
        private object[] _data;
        private string _additional;

        public int page
        {
            get { return _page; }
            set { _page = value; }
        }

        public decimal total
        {
            get { return _total; }
            set { _total = value; }
        }

        public int records
        {
            get { return _records; }
            set { _records = value; }
        }

        public object[] rows
        {
            get { return _data; }
            set { _data = value; }
        }

        public string additional
        {
            get { return _additional; }
            set { _additional = value; }
        }
    }
}