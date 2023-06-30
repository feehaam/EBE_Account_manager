using EcommerceBackend.DTO;
using EcommerceBackend.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateAccountController : ControllerBase
    {

        private readonly IUserCrud _userCrud;
        private readonly Itokens _tokens;

        public CreateAccountController(IUserCrud userCrud, Itokens tokens)
        {
            _userCrud = userCrud;
            _tokens = tokens;
        }

        [HttpPost("/signup")]
        public async Task<IActionResult> CreateAccount(CreateUserDto userDto)
        {
            var result = await _userCrud.Create(userDto);
            try
            {
                if ((int)result == 1)
                {
                    await _tokens.GetEmailVerificationToken(userDto.Email);
                    return Ok("Account created succesfully. Check your email for account verification link.");
                }
                else if ((int)result == 2)
                {
                    return BadRequest("Email already registered.");
                }
                else return BadRequest("Unknown error");
            }
            catch
            {
                return BadRequest(result);
            }
        }

        [HttpPost("/get_email_token")]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            var result = await _tokens.GetEmailVerificationToken(email);
            if(result.Equals("Email is already verified."))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
