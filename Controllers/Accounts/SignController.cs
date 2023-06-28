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

        public SignController(IUserAccount account)
        {
            _account = account;
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
    }
}
