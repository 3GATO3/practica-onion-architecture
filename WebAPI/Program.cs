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
using System;
using Microsoft.EntityFrameworkCore;
using Identity.Context;

namespace WebAPI
{
    public class Program
    {
        public  static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            //net 5
            //var host = CreateHostBuilder(args).Build();
            //using (var scope = host.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            //        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            //        await DefaultRoles.SeedAsync(userManager, roleManager);
            //        await DefaultAdminUser.SeedAsync(userManager, roleManager);
            //        await DefaultBasicUser.SeedAsync(userManager, roleManager);

            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //}

            // net6
            //using (var scope = builder.Build().Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    try
            //    {
            //        var usermanager = services.GetRequiredService<UserManager<ApplicationUser>>();

            //        var rolemanager = services.GetRequiredService<RoleManager<IdentityRole>>();
            //        await DefaultRoles.SeedAsync(usermanager, rolemanager);
            //        await DefaultAdminUser.SeedAsync(usermanager, rolemanager);
            //        await DefaultBasicUser.SeedAsync(usermanager, rolemanager);
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }
            //}

            






            // Add services to the container.
            builder.Services.AddApplicationLayer();
            builder.Services.AddSharedInfraestructure(builder.Configuration);

            builder.Services.AddPersistenceInfraestructure(builder.Configuration);
            builder.Services.AddIdentityInfraestructure(builder.Configuration);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddApiVersioningExtension();
            builder.Services.AddSwaggerGen();
            


            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                //Resolve ASP .NET Core Identity with DI help
                var userManager = (UserManager<ApplicationUser>)scope.ServiceProvider.GetService(typeof(UserManager<ApplicationUser>));
                var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));
                // do you things here
                await DefaultRoles.SeedAsync(userManager, roleManager);
                await DefaultAdminUser.SeedAsync(userManager, roleManager);
                await DefaultBasicUser.SeedAsync(userManager, roleManager);
            }

            //Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();

            app.UseHttpsRedirection();
            
            app.UseAuthorization();
            app.UseErrorHandlingMiddleware();

            app.MapControllers();

            app.Run();
        }
    }
}