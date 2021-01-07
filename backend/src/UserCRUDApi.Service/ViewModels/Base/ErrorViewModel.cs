using System;

namespace UserCRUDApi.Service.ViewModels
{
    public class ErrorViewModel : BaseViewModel
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
