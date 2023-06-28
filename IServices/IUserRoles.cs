using EcommerceBackend.Models;

namespace EcommerceBackend.IServices
{
    public interface IUserRoles
    {
        public Task<string> GetUserRole(string username);
        public Task<string> GetAllRoles();
        public Task<Object> AssignRole(User user, string role);
        public Task<Object> RemoveFromRole(User user, string role);
        public Task<Object> AssignRole(string username, string role);
        public Task<Object> RemoveFromRole(string username, string role);
    }
}
