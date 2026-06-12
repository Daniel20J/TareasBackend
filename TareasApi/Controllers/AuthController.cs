using Microsoft.AspNetCore.Mvc;
using TareasApi.DTOs;
using TareasApi.Services;

namespace TareasApi.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registrarUsuario")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] RegistroDto dto)
        {
            try
            {
                var resultado = await _authService.RegistrarAsync(dto);

                if (!resultado.ok)
                {
                    return BadRequest(new ApiResponseDto<object>
                    {
                        Estatus = false,
                        Mensaje = "No se pudo registrar el usuario.",
                        Error = resultado.error,
                        Data = null
                    });
                }

                return Ok(new ApiResponseDto<object>
                {
                    Estatus = true,
                    Mensaje = "Usuario registrado correctamente.",
                    Error = "",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Estatus = false,
                    Mensaje = "Error interno del servidor.",
                    Error = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("loginUsuario")]
        public async Task<IActionResult> LoginUsuario([FromBody] LoginDto dto)
        {
            try
            {
                var resultado = await _authService.LoginAsync(dto);

                if (!resultado.ok)
                {
                    return BadRequest(new ApiResponseDto<object>
                    {
                        Estatus = false,
                        Mensaje = "No se pudo iniciar sesión.",
                        Error = resultado.error,
                        Data = null
                    });
                }

                return Ok(new ApiResponseDto<AuthResponseDto>
                {
                    Estatus = true,
                    Mensaje = "Login correcto.",
                    Error = "",
                    Data = resultado.data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponseDto<object>
                {
                    Estatus = false,
                    Mensaje = "Error interno del servidor.",
                    Error = ex.Message,
                    Data = null
                });
            }
        }
    }
}
