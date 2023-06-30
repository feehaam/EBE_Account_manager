using EcommerceBackend.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceBackend.Controllers.Utilities_and_Test
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public UtilitiesController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("/utilities/email")]
        public IActionResult SendMail(string andress, string subject, string body)
        {
            return Ok(_emailService.SentMail(andress, subject, body));
        }
    }
}
