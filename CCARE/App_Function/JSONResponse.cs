/* Created By Ardi (20151028) */
namespace CCARE.App_Function
{
    public struct JSONResponse
    {
        private string _response;
        private object _value;
        private object[] _data;

        public string Response
        {
            get { return _response; }
            set { _response = value; }
        }

        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public object[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}