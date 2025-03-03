using B3.Infra.Data.Context;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using B3.Domain.Interfaces;
using B3.Infra.Data.Repositories;
using B3.Application.Services;

namespace B3.Infra.Data
{
    public static class ServiceExtensions
    {
        
        public static void ConfigurePersistenceApp(this IServiceCollection services, IConfiguration configuration)
        {
            //Database connection
            var conn = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options => options
            .UseSqlServer(conn));          
            
        }

        public static void ConfigureInjectionDependencyApp(this IServiceCollection services)
        {
            //Dependency Injection
            services.AddScoped<ITituloService, TituloService>();
            services.AddScoped<ITituloRepository, TituloRepository>();
            services.AddScoped<IDescontoImpostoRendaService, DescontoImpostoRendaService>();
            services.AddScoped<IDescontoImpostoRendaRepository, DescontoImpostoRendaRepository>();

        }
    }
}
