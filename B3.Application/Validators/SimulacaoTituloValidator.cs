using B3.Application.Inputs;
using B3.Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3.Application.Validators
{
    public class SimulacaoTituloValidator : AbstractValidator<SimulacaoTituloInputModel>
    {
        public SimulacaoTituloValidator()
        {
            RuleFor(x => x.IdTitulo)
                .NotNull().NotEmpty().WithMessage("O Titulo é obrigatório para simulação");

            RuleFor(x => x.ValorInicial)
                .NotNull().NotEmpty().WithMessage("O Valor Inicial deve ser informado")
                .GreaterThan(0).WithMessage("O Valor Inicial não pode ser zero");
                

            RuleFor(x => x.QuantidadeMesesInvestimento)
                .NotNull().NotEmpty().WithMessage("A quantidade de meses para o investimento deve ser informada")
                .GreaterThanOrEqualTo(1).WithMessage("A quantidade de meses deve ser no minimo igual a um");
                
                

        }
    }
}
