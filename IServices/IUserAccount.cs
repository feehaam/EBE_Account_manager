using EcommerceBackend.DT;

namespace EcommerceBackend.IServices
{
    public interface IUserAccount 
    {
        public Task<Object> Login(SignInDto signDto);
        public Task<string> Logout();
        public Task<string> VerifyEmail(string token, string email);
        public Task<string> ResetPassword(string email, string token, string password);
    }
}
