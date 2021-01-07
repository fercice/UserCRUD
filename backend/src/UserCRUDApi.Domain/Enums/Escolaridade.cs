using System.ComponentModel.DataAnnotations;

namespace UserCRUDApi.Domain.Enums
{
    public enum Escolaridade
    {        
        [Display(Name = "Infantil")]
        Infantil = 1,
        [Display(Name = "Fundamental")]
        Fundamental = 2,
        [Display(Name = "Médio")]
        Medio = 3,
        [Display(Name = "Superior")]
        Superior = 4,
    }
}
