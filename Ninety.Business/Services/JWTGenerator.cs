﻿using Microsoft.IdentityModel.Tokens;
using Ninety.Models.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ninety.Business.Services
{
    public class JWTGenerator
    {
        public static string GenerateToken(UserDTO account)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("1c4890495b93b9e71fee12bf1880242771ad287f814d9553b120de5b82428b0b");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", account.Id.ToString()),
                    new Claim("name", account.Name),
                    new Claim("email", account.Email),
                    new Claim("password", account.Password),
                    new Claim("gender", account.Gender.ToString()),
                    new Claim(ClaimTypes.Role, account.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(100),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private static List<InvalidToken> InvalidTokens = new List<InvalidToken>();

        public static void InvalidateToken(string token)
        {
            InvalidTokens.Add(new InvalidToken { Token = token, InvalidatedAt = DateTime.UtcNow });
        }

        public static bool IsTokenValid(string token)
        {
            ClearExpiredTokens();
            return !InvalidTokens.Any(t => t.Token == token);
        }

        private static void ClearExpiredTokens()
        {
            var expirationThreshold = DateTime.UtcNow.AddHours(-1);

            InvalidTokens.RemoveAll(t => t.InvalidatedAt < expirationThreshold);
        }

        private class InvalidToken
        {
            public string Token { get; set; }
            public DateTime InvalidatedAt { get; set; }
        }
    }
}
