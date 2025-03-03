using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft;
using Microsoft.Extensions.DependencyInjection;
using B3.Infra.Data;
using B3.Infra.Data.Context;


namespace B3.Application.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.ConfigurePersistenceApp(builder.Configuration);
            builder.Services.ConfigureInjectionDependencyApp();

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("AllowAngular", policy => policy.WithOrigins("http://localhost:50091").AllowAnyHeader().AllowAnyMethod());
            });


            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "B3.API", Version = "v1" });
                });





            var app = builder.Build();
            app.UseCors("AllowAngular");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                
                using IServiceScope scope = app.Services.CreateScope();

                using AppDbContext dbContext =
                    scope.ServiceProvider.GetRequiredService<AppDbContext>();

                dbContext.Database.Migrate();
            }




            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }


    }
}
