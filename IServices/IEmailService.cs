namespace EcommerceBackend.IServices
{
    public interface IEmailService
    {
        public string SentMail(string email, string subject, string body);
    }
}
