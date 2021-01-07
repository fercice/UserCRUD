using System;
using System.ComponentModel.DataAnnotations;

namespace UserCRUDApi.Service.ViewModels
{
    public class BaseViewModel
    {
        [Key]
        public int Id { get; set; }
    }
}
