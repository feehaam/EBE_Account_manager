using Azure.Core;
using EcommerceBackend.IServices;
using EcommerceBackend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceBackend.Services
{
    public class Tokens : Itokens
    {
        private string _issuer;
        private string _audience;
        private string _secretKey;
        private int _expirationInMinutes;

        private UserManager<User> _userManager;

        public Tokens(IConfiguration configuration, UserManager<User> userManager)
        {
            _issuer = configuration.GetValue<string>("JwtSettings:Issuer");
            _audience = configuration.GetValue<string>("JwtSettings:Audience");
            _secretKey = configuration.GetValue<string>("JwtSettings:SecretKey");
            _expirationInMinutes = configuration.GetValue<int>("JwtSettings:ExpirationInMinutes");
            _userManager = userManager;
        }

        public async Task<string> GetLoginToken(List<Claim> claims)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var claimsIdentity = new ClaimsIdentity(claims);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(_expirationInMinutes),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GetEmailVerificationToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return "User doesn't exist.";
            }

            var alreadyVerified = await _userManager.IsEmailConfirmedAsync(user);
            if (alreadyVerified)
            {
                return "Email is already verified.";
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }
    }
}