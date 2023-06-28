using EcommerceBackend.DTO;
using EcommerceBackend.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreateAccountController : ControllerBase
    {

        private readonly IUserCrud _userCrud;

        public CreateAccountController(IUserCrud userCrud)
        {
            _userCrud = userCrud;
        }

        [HttpPost("/signup")]
        public async Task<IActionResult> CreateAccount(CreateUserDto userDto)
        {
            var result = await _userCrud.Create(userDto);
            try
            {
                if ((int)result == 1)
                {
                    return Ok("Account created succesfully.");
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
    }
}
