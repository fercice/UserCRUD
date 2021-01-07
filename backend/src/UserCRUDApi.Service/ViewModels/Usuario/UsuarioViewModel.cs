using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UserCRUDApi.Domain.Enums;

namespace UserCRUDApi.Service.ViewModels
{
    public class UsuarioViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]        
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Sobrenome é obrigatório")]
        [DisplayName("Sobrenome")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Data de Nascimento é obrigatório")]        
        [DisplayName("Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Escolaridade é obrigatório")]
        [DisplayName("Escolaridade")]
        public Escolaridade Escolaridade { get; set; }
        
        public string escolaridadeNome { get; set; }
    }
}
