using B3.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3.Application.Validators
{
    public class TituloValidator : AbstractValidator<Titulo>
    {
        public TituloValidator()
        {
            RuleFor(x => x.NomeTitulo)
                .NotEmpty().WithMessage("O nome do título deve ser preenchido.");

            RuleFor(x => x.IdTipoTitulo)
                .NotEmpty().WithMessage("O tipo de titulo é obrigatório.");

            RuleFor(x => x.TaxaRendimento)
                .NotEmpty().WithMessage("A taxa de rendimento do titulo deve ser informada.")
                .GreaterThan(0).WithMessage("O valor da taxa de rendimento deve ser maior que zero.");
        }
    }
}
