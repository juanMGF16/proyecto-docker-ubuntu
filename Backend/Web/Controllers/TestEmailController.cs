using Business.Services.SendEmail.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Utilities.Templates;

namespace Web.Controllers
{
    [ApiController]
    [Route("api/test/email")]
    public class TestEmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<TestEmailController> _logger;

        public TestEmailController(IEmailService emailService, ILogger<TestEmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("test-template")]
        public IActionResult TestEmailTemplate([FromBody] TestTemplateRequest request)
        {
            try
            {
                var htmlContent = EmailTemplates.GetPasswordRecoveryTemplate(
                    request.Username,
                    request.RecoveryLink,
                    request.ExpirationHours
                );

                return Ok(new
                {
                    success = true,
                    message = "Plantilla generada correctamente",
                    html = htmlContent,
                    preview = "Revisa el HTML generado en la propiedad 'html'"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando plantilla de email");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error generando plantilla",
                    error = ex.Message
                });
            }
        }

        [HttpPost("send-test-email")]
        public async Task<IActionResult> SendTestEmail([FromBody] SendTestEmailRequest request)
        {
            try
            {
                var subject = "✅ Prueba de Plantilla - Sistema de Notificaciones";
                var body = EmailTemplates.GetPasswordRecoveryTemplate(
                    request.Username,
                    request.RecoveryLink,
                    request.ExpirationHours
                );

                var result = await _emailService.SendEmailAsync(
                    request.ToEmail,
                    subject,
                    body,
                    true
                );

                if (result)
                {
                    return Ok(new
                    {
                        success = true,
                        message = $"Email de prueba enviado a: {request.ToEmail}",
                        details = "Revisa tu bandeja de entrada (y spam)"
                    });
                }

                return BadRequest(new
                {
                    success = false,
                    message = "Error al enviar el email de prueba",
                    details = "Verifica la configuración de SMTP"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando email de prueba");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error enviando email de prueba",
                    error = ex.Message,
                    details = ex.StackTrace
                });
            }
        }

        [HttpPost("test-welcome-template")]
        public IActionResult TestWelcomeTemplate([FromBody] TestWelcomeTemplateRequest request)
        {
            try
            {
                var htmlContent = EmailTemplates.GetWelcomeTemplate(
                    request.Username,
                    request.LoginLink
                );

                return Ok(new
                {
                    success = true,
                    message = "Plantilla de bienvenida generada correctamente",
                    html = htmlContent
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando plantilla de bienvenida");
                return StatusCode(500, new
                {
                    success = false,
                    message = "Error generando plantilla de bienvenida",
                    error = ex.Message
                });
            }
        }
    }

    public class TestTemplateRequest
    {
        public string Username { get; set; } = "Usuario de Prueba";
        public string RecoveryLink { get; set; } = "https://tusistema.com/recovery?token=abc123";
        public int ExpirationHours { get; set; } = 24;
    }

    public class SendTestEmailRequest
    {
        public string ToEmail { get; set; } = string.Empty;
        public string Username { get; set; } = "Usuario de Prueba";
        public string RecoveryLink { get; set; } = "https://tusistema.com/recovery?token=test123";
        public int ExpirationHours { get; set; } = 24;
    }

    public class TestWelcomeTemplateRequest
    {
        public string Username { get; set; } = "Usuario de Prueba";
        public string LoginLink { get; set; } = "https://tusistema.com/login";
    }
}
