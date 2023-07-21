using Aplication;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting;
using Persistence;
using Shared;
using System.Reflection;
using MediatR;
using WebAPI.Extensions;
using Microsoft.AspNetCore.Identity;
using Identity.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace WebAPI
{
    public class Program
    {
        public  static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


           
                // builder.Services.AddScoped<UserManager<ApplicationUser>>();
                //builder.Services.AddScoped<RoleManager<IdentityRole>>();
            
           

            


            // Add services to the container.
            builder.Services.AddApplicationLayer();
            builder.Services.AddSharedInfraestructure(builder.Configuration);

            builder.Services.AddPersistenceInfraestructure(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddApiVersioningExtension();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseErrorHandlingMiddleware();


            app.MapControllers();

            app.Run();
        }
    }
}