using B3.Application.Services;
using B3.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using B3.Application.Validators;
using FluentValidation;

namespace B3.Application
{
    public static class ValidationExtensions
    {
        public static void ConfigureValidationDependencies(this IServiceCollection services)
        {
            //Dependency Injection
            services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<SimulacaoTituloValidator>();

        }
    }
}
