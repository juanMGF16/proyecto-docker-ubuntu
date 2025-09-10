    using Microsoft.AspNetCore.Mvc;
    using Utilities.Exceptions;

    namespace Web.Controllers.Base
    {
        [ApiController]
        [Produces("application/json")]
        public abstract class BaseController<TService> : ControllerBase
        {
            protected readonly TService _service;
            protected readonly ILogger _logger;

            protected BaseController(TService service, ILogger logger)
            {
                _service = service;
                _logger = logger;
            }

            protected async Task<IActionResult> TryExecuteAsync<T>(Func<Task<T>> func, string context)
            {
                try
                {
                    var result = await func();
                    return Ok(result);
                }
                catch (ValidationException ex)
                {
                    _logger.LogWarning(ex, $"{context} - Validación");
                    return BadRequest(new { message = ex.Message });
                }
                catch (EntityNotFoundException ex)
                {
                    _logger.LogInformation(ex, $"{context} - No encontrado");
                    return NotFound(new { message = ex.Message });
                }
                catch (ForbiddenException ex)
                {
                    _logger.LogWarning(ex, $"{context} - Forbidden");
                    return Forbid(); 
                }
            catch (Exception ex)
                {
                    _logger.LogError(ex, $"{context} - Error general");
                    return StatusCode(500, new { message = "Ocurrió un error inesperado." });
                }
            }

            protected async Task<IActionResult> TryExecuteAsync(Func<Task<IActionResult>> func, string context)
            {
                try
                {
                    return await func();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"{context} - Error general");
                    return StatusCode(500, new { message = "Error inesperado." });
                }
            }
        }
    }
