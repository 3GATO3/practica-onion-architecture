using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interfaces;
using Aplication.Wrappers;
using Application.DTOs.Users;
using Microsoft.AspNetCore.Identity;
using Identity.Models;
using Domain.Settings;
using Aplication.Interfaces;
using Aplication.Exceptions;
using Aplication.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Identity.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;

namespace Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;

        public AccountService(IDateTimeService dateTimeService, IOptions<JWTSettings> jwtSettings, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._dateTimeService = dateTimeService;
            _jwtSettings = jwtSettings.Value;
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
        
            var usuario = await _userManager.FindByEmailAsync(request.Email);
            if (usuario == null)
            {
                throw new ApiException($"No hay una cuenta registrada con este correo {request.Email}");
            }
            var result = await _signInManager.PasswordSignInAsync(usuario.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded) 
            {
                throw new ApiException($"Las credenciales del usuario no son validas {request.Email}");
            }
            JwtSecurityToken jwtSecurityToken = await GenerateJWTToken(usuario);
            AuthenticationResponse response= new AuthenticationResponse();

            response.Id = usuario.Id;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.Email = usuario.Email;
            response.UserName = usuario.UserName;

            var roleList = await _userManager.GetRolesAsync(usuario).ConfigureAwait(false);
            response.Roles = roleList.ToList();
            response.IsVerified = usuario.EmailConfirmed;

            var refreshToken = GenerateRefreshToken(ipAddress);
            response.RefreshToken = refreshToken.Token;
            return new Response<AuthenticationResponse>(response, $"Usuario autenticado {usuario.UserName}");
        }

    public async Task<Response<string>>RegisterAsync(RegisterRequest request,string origin)
    {
            var usuarioConElMismoUserName = await _userManager.FindByNameAsync(request.UserName);
            if(usuarioConElMismoUserName != null)
            { 
                throw new ApiException($"El nombre de usuario {request.UserName} ya fue registrado previamente.");
            }

            var usuario = new ApplicationUser
            {
                Email = request.Email,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                UserName = request.UserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            var usuarioConElMismoCorreo = await _userManager.FindByEmailAsync(request.Email);
            if (usuarioConElMismoCorreo != null)
            { throw new ApiException($"El email {request.Email} ya fue registrado peviamente.");
            }
            else
            {
                var result = await _userManager.CreateAsync(usuario, request.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(usuario, Roles.Basic.ToString());
                    return new Response<string>(usuario.Id, message: $"UsuarioRegistradoExitosamente.{request.UserName}");
                }
                else
                { throw new ApiException($"{result.Errors}.");
                }
            }
    }


        public async Task<JwtSecurityToken> GenerateJWTToken(ApplicationUser usuario)
        {
            var userClaims = await _userManager.GetClaimsAsync(usuario);
            var roles = await _userManager.GetRolesAsync(usuario);
            var roleClaims = new List<Claim>();

            for (int i=0; i<roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            string ipAddress = IpHelper.GetIpAddress();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim("uid", usuario.Id),
                new Claim("ip", ipAddress),
            }
           .Union(userClaims)
            .Union(roleClaims) ;
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        public RefreshToken GenerateRefreshToken(string ipAddress)
        { return new RefreshToken
        {
            Token = RandomTokenString(),
            Expires = DateTime.Now.AddDays(7), 
                Created = DateTime.Now,
                CreatedByIp = ipAddress
        };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceprovider = new RNGCryptoServiceProvider();
            var RandomBytes = new Byte[40];
            rngCryptoServiceprovider.GetBytes(RandomBytes);
            return BitConverter.ToString(RandomBytes).Replace("_","");
        }
    
    }
}
