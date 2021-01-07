using System;
using UserCRUDApi.Domain.Enums;

namespace UserCRUDApi.Domain.Entities
{
    public class Usuario : Entity
    {
        // Empty constructor for EF
        protected Usuario() { }

        public string Nome { get; set; }

        public string Sobrenome { get; set; }

        public string Email { get; set; }

        public DateTime DataNascimento { get; set; }

        public Escolaridade Escolaridade { get; set; }
    }
}