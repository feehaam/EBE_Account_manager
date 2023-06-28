using System.Security.Claims;

namespace EcommerceBackend.IServices
{
    public interface Itokens
    {
        public Task<string> GetLoginToken(List<Claim> claims);
        public Task<string> GetEmailVerificationToken(string email);
    }
}
