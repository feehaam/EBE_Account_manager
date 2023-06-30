using EcommerceBackend.DTO;
using EcommerceBackend.Helpers;
using EcommerceBackend.IServices;
using EcommerceBackend.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace EcommerceBackend.Services
{
    public class UserCrud : IUserCrud
    {
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IUserRoles _userRoles;
        private Itokens _tokens;

        public UserCrud(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, 
            IUserRoles userRoles, Itokens tokens)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userRoles = userRoles;
            _tokens = tokens;
        }

        async public Task<Object> Create(CreateUserDto userDto)
        {
            bool RolesAdded = await AddRoles();
            if (!RolesAdded)
            {
                return "Failed to add roles";
            }

            var user = new User();
            user.Age = userDto.Age;
            user.Email = userDto.Email;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.EmailConfirmed = false;
            user.UserName = userDto.Email;
            user.NID = "N/A";
            user.ProfilePicURL = "N/A";
            
            if(await _userManager.FindByEmailAsync(userDto.Email) != null)
            {
                return 2;
            }

            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await _userRoles.AssignRole(user, "customer");
                return 1;
            }
            else
            {
                return "Please check your email to confirm account";
            }
        }

        static bool rolesAdditionComplete = false;
        private async Task<bool> AddRoles()
        {
            if (rolesAdditionComplete) return true;

            foreach (string role in Actor.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            rolesAdditionComplete = true;

            foreach (string role in Actor.Roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    rolesAdditionComplete = false;
                    return false;
                }
            }

            return rolesAdditionComplete;
        }

        public async Task<object> Read(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return "User doesn't exist";
            }
            else
            {
                ReadUserDto userDto = new ReadUserDto();

                userDto.Email = user.Email;
                userDto.ProfilePicURL = user.ProfilePicURL;
                userDto.FirstName = user.FirstName;
                userDto.LastName = user.LastName;
                userDto.Age = user.Age;
                userDto.ID = user.Id;
                userDto.Roles = await _userRoles.GetUserRole(email);

                return userDto;
            }
        }

        public async Task<object> Update(string email, UpdateUserDto userDto)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                return "User doesn't exist.";
            }
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.Age = userDto.Age;
            user.NID = userDto.NID;
            user.ProfilePicURL = userDto.ProfilePicURL;
            var result = await _userManager.UpdateAsync(user);
            return result;
        }

        public async Task<object> Delete(string email)
        {
            User user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return "User doesn't exist.";
            }
            return await _userManager.DeleteAsync(user);
        }
    }
}
