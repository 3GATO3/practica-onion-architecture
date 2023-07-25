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
using Identity;
using Identity.Seeds;

namespace WebAPI
{
    public class Program
    {
        public  static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // builder.Services.AddScoped<UserManager<ApplicationUser>>();
            //builder.Services.AddScoped<RoleManager<IdentityRole>>();
            try
            {
                //var userManager = builder.Services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
                //var roleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();

                //await DefaultRoles.SeedAsync(userManager, roleManager);
                //await DefaultAdminUser.SeedAsync(userManager, roleManager);
                //await DefaultBasicUser.SeedAsync(userManager, roleManager);


            }
            catch (Exception)
            {

                throw;
            }
          
            // Add services to the container.
            builder.Services.AddApplicationLayer();
           // builder.AddIdentityInfraestructure(builder.Configuration);
            builder.Services.AddSharedInfraestructure(builder.Configuration);

            builder.Services.AddPersistenceInfraestructure(builder.Configuration);
            builder.Services.AddIdentityInfraestructure(builder.Configuration);

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
            app.UseAuthentication();

            app.MapControllers();

            app.Run();
        }
    }
}