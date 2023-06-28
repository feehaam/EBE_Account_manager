using EcommerceBackend.DTO;
using EcommerceBackend.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserReadUpdateDeleteController : ControllerBase
    {
        private readonly IUserCrud _userCrud;

        public UserReadUpdateDeleteController(IUserCrud userCrud)
        {
            _userCrud = userCrud;
        }


        [HttpGet("/user/{email}")]
        public async Task<IActionResult> GetUser(string email)
        {
            var user = await _userCrud.Read(email);
            return Ok(user);
        }

        [HttpPut("/user/update")]
        public async Task<IActionResult> UpdateUser(string email, UpdateUserDto userDto)
        {
            var result = await _userCrud.Update(email, userDto);
            return Ok(result);
        }

        [HttpPut("/user/delete")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var result = await _userCrud.Delete(email);
            return Ok(result);
        }


    }
}
