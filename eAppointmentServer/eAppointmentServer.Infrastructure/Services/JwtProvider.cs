﻿using eAppointmentServer.Application.Services;
using eAppointmentServer.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace eAppointmentServer.Infrastructure.Services;
internal sealed class JwtProvider(IConfiguration configuration) : IJwtProvider
{
    public string CreateToken(AppUser user)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.FullName),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new("UserName", user.UserName ?? string.Empty)
        ];

        DateTime expires = DateTime.Now.AddDays(1);

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:SecretKey").Value ?? ""));

        SigningCredentials signinCredentials = new(securityKey, SecurityAlgorithms.HmacSha512);

        JwtSecurityToken securityToken = new(
            issuer: configuration.GetSection("Jwt:Issuer").Value,
            audience: configuration.GetSection("Jwt:Audience").Value,
            claims: claims,
            notBefore: DateTime.Now,
            expires: expires,
            signingCredentials: signinCredentials
            );

        JwtSecurityTokenHandler securityTokenHandler = new();

        string token = securityTokenHandler.WriteToken(securityToken);

        return token;

    }
}
