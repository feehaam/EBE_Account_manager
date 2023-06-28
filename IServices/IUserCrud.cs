using EcommerceBackend.DTO;
using EcommerceBackend.Helpers;

namespace EcommerceBackend.IServices
{
    public interface IUserCrud
    {
        public Task<Object> Create(CreateUserDto userDto);
        public Task<Object> Read(string email);
        public Task<Object> Update(string email, UpdateUserDto userDto);
        public Task<Object> Delete(string email);
    }
}
