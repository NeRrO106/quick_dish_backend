using Microsoft.AspNetCore.Mvc;
using QUickDish.API.DTOs;
using QUickDish.API.Services;

namespace QUickDish.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : Controller
    {

        private readonly EmailService _emailService;

        public ContactController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        public async Task<IActionResult> ContactUs([FromBody] ContactRequest contactForm)
        {
            if (contactForm == null ||
                string.IsNullOrWhiteSpace(contactForm.Name) ||
                string.IsNullOrWhiteSpace(contactForm.Email) ||
                string.IsNullOrWhiteSpace(contactForm.Message))
            {
                return BadRequest("All fields are required.");
            }

            var subject = $"New Contact Message from {contactForm.Name}";

            var body = $"<h1>Contact Message</h1>" +
                       $"<p><strong>Name:</strong> {contactForm.Name}</p>" +
                       $"<p><strong>Email:</strong> {contactForm.Email}</p>" +
                       $"<p><strong>Message:</strong><br/>{contactForm.Message}</p>";
            await _emailService.SendEmailAsync(
                "contact.quickdish@gmail.com",
                subject,
                body
            );

            return Ok("Message sent successfully.");
        }
    }
}
