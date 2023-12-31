﻿using Aplication.Wrappers;
using Application.Interfaces;
using Domain.Settings;
using Identity.Context;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity
{
    public static class ServiceExtensions
    {
        public static void AddIdentityInfraestructure(this IServiceCollection services,   IConfiguration configuration)
        {
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(
            configuration.GetConnectionString("IdentityConnection"),
            b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));


            services.AddIdentity<ApplicationUser, IdentityRole>().AddSignInManager<SignInManager<ApplicationUser>>().
                AddUserManager<UserManager<ApplicationUser>>().
                AddRoles<IdentityRole>().
                AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();
            
            
            
            #region Services
            services.AddTransient<IAccountService,AccountService>();
            #endregion
            services.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            ).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:key"]))
                };


                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();
                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    },
                    OnChallenge = context =>
                    {
                        if (!context.Response.HasStarted)
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Response<string>("Usted no está autorizado"));
                            return context.Response.WriteAsync(result);
                       }
                        else
                        return context.Response.WriteAsync(string.Empty);
                    },

                    OnForbidden = Context =>
                    {
                        Context.Response.StatusCode = 400;
                        Context.Response.ContentType = "application/json";
                        var result = JsonConvert.SerializeObject(new Response<string>("Usted no tiene permisos sobre este recurso"));
                        return Context.Response.WriteAsync(result);
                    }

                };
            }
            );
        }
    }
}
