using EcommerceBackend.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesManagementController : ControllerBase
    {

        private readonly IUserRoles _roles;

        public RolesManagementController(IUserRoles roles)
        {
            _roles = roles;
        }

        [HttpGet("/roles/")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _roles.GetAllRoles());
        }

        [HttpGet("/roles/user/{username}")]
        public async Task<IActionResult> GetUserRoles(string username)
        {
            return Ok(await _roles.GetUserRole(username));
        }


        [HttpPost("/roles/add_customer")]
        public async Task<IActionResult> AddCustomer(string email)
        {
            return Ok(await _roles.AssignRole(email, "customer"));
        }

        [HttpPost("/roles/remove_customer")]
        public async Task<IActionResult> RemoveCustomer(string email)
        {
            return Ok(await _roles.RemoveFromRole(email, "customer"));
        }

        [HttpPost("/roles/add_shop")]
        public async Task<IActionResult> AddShop(string email)
        {
            return Ok(await _roles.AssignRole(email, "shop"));
        }

        [HttpPost("/roles/remove_shop")]
        public async Task<IActionResult> RemoveShop(string email)
        {
            return Ok(await _roles.RemoveFromRole(email, "shop"));
        }

        [HttpPost("/roles/add_delivery")]
        public async Task<IActionResult> AddDelivery(string email)
        {
            return Ok(await _roles.AssignRole(email, "delivery"));
        }

        [HttpPost("/roles/remove_delivery")]
        public async Task<IActionResult> RemoveDelivery(string email)
        {
            return Ok(await _roles.RemoveFromRole(email, "delivery"));
        }


        [HttpPost("/roles/add_moderator")]
        public async Task<IActionResult> AddModerator(string email)
        {
            return Ok(await _roles.AssignRole(email, "moderator"));
        }

        [HttpPost("/roles/remove_moderator")]
        public async Task<IActionResult> RemoveModerator(string email)
        {
            return Ok(await _roles.RemoveFromRole(email, "moderator"));
        }

        [HttpPost("/roles/add_admin")]
        public async Task<IActionResult> AddAdmin(string email)
        {
            return Ok(await _roles.AssignRole(email, "admin"));
        }

        [HttpPost("/roles/remove_admin")]
        public async Task<IActionResult> RemoveAdmin(string email)
        {
            return Ok(await _roles.RemoveFromRole(email, "admin"));
        }

    }
}
