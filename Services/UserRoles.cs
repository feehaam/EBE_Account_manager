using EcommerceBackend.Helpers;
using EcommerceBackend.IServices;
using EcommerceBackend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace EcommerceBackend.Services
{
    public class UserRoles : IUserRoles
    {
        private UserManager<User> _userManager;

        public UserRoles(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        async public Task<object> AssignRole(User user, string role)
        {
            if (!Actor.Roles.Contains(role))
            {
                return "Role doesn't exist";
            }
            var result = await _userManager.AddToRoleAsync(user, role);
            return result;
        }

        async public Task<object> AssignRole(string username, string role)
        {
            if(username == null)
            {
                return "User doesn't exist";
            }

            User user = await _userManager.FindByNameAsync(username);
            if(user == null)
            {
                return "User doesn't exist";
            }

            return await AssignRole(user, role);
        }

        public async Task<string> GetAllRoles()
        {
            return string.Join(", ", Actor.Roles);
        }

        async public Task<string> GetUserRole(string username)
        {
            User user = await _userManager.FindByEmailAsync(username);
            if (user == null)
            {
                return "User doesn't exist";
            }
            else
            {
                var roles = await _userManager.GetRolesAsync(user);
                if(roles == null || roles.Count == 0)
                {
                    return "N/A";
                }
                return string.Join(", ", roles);
            }
        }

        async public Task<object> RemoveFromRole(User user, string role)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result;
        }

        async public Task<object> RemoveFromRole(string username, string role)
        {
            if (username == null)
            {
                return "User doesn't exist";
            }

            User user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return "User doesn't exist";
            }

            return await RemoveFromRole(user, role);
        }
    }
}
