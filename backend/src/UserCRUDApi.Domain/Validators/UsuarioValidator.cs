using System;
using FluentValidation;
using UserCRUDApi.Domain.Entities;

namespace UserCRUDApi.Domain.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(c => c)
                .NotNull()
                .OnAnyFailure(x =>
                {
                    throw new ArgumentNullException("Nenhum dado encontrado.");
                });

            RuleFor(c => c.Nome)
                .NotEmpty().NotNull().WithMessage("Nome é obrigatório.");

            RuleFor(c => c.Sobrenome)
                .NotEmpty().NotNull().WithMessage("Sobrenome é obrigatório.");

            RuleFor(c => c.Email)
                .NotEmpty().NotNull().WithMessage("E-mail é obrigatório.");

            RuleFor(c => c.DataNascimento)
                .NotEmpty().NotNull().WithMessage("Data de Nascimento é obrigatório.");

            RuleFor(c => c.Escolaridade)
                .NotEmpty().NotNull().WithMessage("Escolaridade é obrigatório.");
        }
    }
}