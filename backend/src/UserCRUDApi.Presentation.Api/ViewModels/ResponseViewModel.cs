using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace UserCRUDApi.Presentation.Api.ViewModels
{
    public class ResponseViewModel
    {
        public ResponseViewModel()
        {
            HttpStatusCode = HttpStatusCode.OK;
        }

        public ResponseViewModel(object data) : this() => Data = data;

        public ResponseViewModel(HttpStatusCode httpStatusCode, object data)
        {
            HttpStatusCode = httpStatusCode;
            Data = data;
        }

        public ResponseViewModel(HttpStatusCode httpStatusCode, string successMessage)
        {
            HttpStatusCode = httpStatusCode;
            SuccessMessage = successMessage;
        }

        public ResponseViewModel(HttpStatusCode httpStatusCode, List<string> errors)
        {
            HttpStatusCode = httpStatusCode;
            Errors = errors;
        }

        public HttpStatusCode HttpStatusCode { get; set; }

        public object Data { get; set; }        

        public string SuccessMessage { get; set; }

        public List<string> Errors = new List<string>();
    }
}
