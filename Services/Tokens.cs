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
        private readonly IHttpContextAccessor _httpContextAccessor;

        private UserManager<User> _userManager;
        private IEmailService _emailService;

        public Tokens(IConfiguration configuration, UserManager<User> userManager, 
            IEmailService emailService, IHttpContextAccessor httpContextAccessor)
        {
            _issuer = configuration.GetValue<string>("JwtSettings:Issuer");
            _audience = configuration.GetValue<string>("JwtSettings:Audience");
            _secretKey = configuration.GetValue<string>("JwtSettings:SecretKey");
            _expirationInMinutes = configuration.GetValue<int>("JwtSettings:ExpirationInMinutes");
            _userManager = userManager;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
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

            var request = _httpContextAccessor.HttpContext.Request;
            var baseUri = $"{request.Scheme}://{request.Host}";
            var confirmationLink = $"{baseUri}/confirm_email?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email)}";

            _emailService.SentMail(
                email: user.Email,
                subject: "Confirm your account.",
                body: "Hello " + user.FirstName + " " + user.LastName + ", Please click the link below to " +
                      "confirm your account at Feehams ecommerce website. \n" + confirmationLink
            );

            return "Check your email for the verification link.";
        }

        public async Task<string> GetForgottenPasswordResetToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return "User doesn't exist";
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            
            var changePasswordLink = $"http://localhost:7108/confirm_email?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email)}";

            _emailService.SentMail(
                email: user.Email,
                subject: "Reset password.",
                body: "Hello " + user.FirstName + " " + user.LastName + ", Please use to token below to " +
                "change your password at Feehams eccomerce web site. \n" + changePasswordLink
                );

            return token;
        }
    }
}