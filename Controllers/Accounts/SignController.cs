using EcommerceBackend.DT;
using EcommerceBackend.IServices;
using EcommerceBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignController : ControllerBase
    {
        private IUserAccount _account;
        private Itokens _tokens;

        public SignController(IUserAccount account, Itokens tokens)
        {
            _account = account;
            _tokens = tokens;
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login(SignInDto signDto)
        {
            return Ok(await _account.Login(signDto));
        }

        [HttpGet("/logout")]
        public async Task<IActionResult> Lougout()
        {
            return Ok(await _account.Logout());
        }

        [HttpGet("/confirm_email")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var result = await _account.VerifyEmail(token, email);
            return Ok(result);
        }

        [HttpPost("/forgotten_password_reset_token")]
        public async Task<IActionResult> FPRT(string email)
        {
            var token = await _tokens.GetForgottenPasswordResetToken(email);
            return Ok(token);
        }

        [HttpPost("/reset_password")]
        public async Task<IActionResult> ResetPassword(string email, string code, string password)
        {
            var result = await _account.ResetPassword(email, code, password);
            return Ok(result);
        }
    }
}
