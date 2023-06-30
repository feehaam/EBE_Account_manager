using EcommerceBackend.DT;
using EcommerceBackend.IServices;
using EcommerceBackend.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace EcommerceBackend.Services
{
    public class UserAccount : IUserAccount
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Itokens _token;

        public UserAccount(UserManager<User> userManager, Itokens token, 
            SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _token = token;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<Object> Login(SignInDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return "User doesn't exist.";
            }

            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                return "Email not verified. Please verify your email before logging in.";
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, true, lockoutOnFailure: false);

            if (signInResult.Succeeded)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email), 
                    new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                };
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                var token = _token.GetLoginToken(authClaims);

                return new { Token = token, Message = "Logged in successfully" };
            }
            else if (signInResult.IsLockedOut)
            {
                return "Account locked due to multiple failed login attempts. Please try again later.";
            }
            else if (signInResult.RequiresTwoFactor)
            {
                return "Two-factor authentication is required for this account.";
            }
            else
            {
                return "Invalid username or password.";
            }
        }

        public async Task<string> Logout()
        {
            var user = await _signInManager.UserManager.GetUserAsync(_signInManager.Context.User);

            await _signInManager.SignOutAsync();

            return "Logged out successfully";
        }


        public async Task<string> VerifyEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return "Invalid email or token.";
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("user"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("user"));
                }
                await _userManager.AddToRoleAsync(user, "user");
                return "Email confirmed successfully.";
            }
            else
            {
                return "Failed to confirm email.";
            }
        }

        public async Task<string> ResetPassword(string email, string token, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return "User doesn't exist";
            }

            var result = await _userManager.ResetPasswordAsync(user, token, password);
            if (result.Succeeded)
            {
                return "Password changed successfully";
            }
            else
            {
                return "Failed to change password";
            }
        }
    }
}
