using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TareasApi.DTOs;
using TareasApi.Services;

namespace TareasApi.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class TareasController : ControllerBase
    {
        private readonly ITareaService _tareaService;

        public TareasController(ITareaService tareaService)
        {
            _tareaService = tareaService;
        }

        [HttpPost("crearTarea")]
        public async Task<IActionResult> CrearTarea([FromBody] TareaCrearDto dto)
        {
            try
            {
                int usuarioId = ObtenerUsuarioIdToken();
                var resultado = await _tareaService.CrearTareaAsync(usuarioId, dto);

                if (!resultado.ok)
                {
                    return BadRequest(new ApiResponseDto<object>
                    {
                        Estatus = false,
                        Mensaje = "No se pudo crear la tarea.",
                        Error = resultado.error,
                        Data = null
                    });
                }

                return Ok(new ApiResponseDto<TareaDto>
                {
                    Estatus = true,
                    Mensaje = "Tarea creada correctamente.",
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

        [HttpGet("getTareas")]
        public async Task<IActionResult> GetTareas()
        {
            try
            {
                int usuarioId = ObtenerUsuarioIdToken();
                var resultado = await _tareaService.GetTareasAsync(usuarioId);

                if (!resultado.ok)
                {
                    return BadRequest(new ApiResponseDto<object>
                    {
                        Estatus = false,
                        Mensaje = "No se pudieron listar las tareas.",
                        Error = resultado.error,
                        Data = null
                    });
                }

                return Ok(new ApiResponseDto<List<TareaDto>>
                {
                    Estatus = true,
                    Mensaje = resultado.data != null && resultado.data.Count > 0
                        ? "Tareas obtenidas correctamente."
                        : "No hay tareas registradas.",
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

        [HttpGet("getTareasId/{id:int}")]
        public async Task<IActionResult> GetTareasId(int id)
        {
            try
            {
                int usuarioId = ObtenerUsuarioIdToken();
                var resultado = await _tareaService.GetTareasIdAsync(usuarioId, id);

                if (!resultado.ok)
                {
                    return NotFound(new ApiResponseDto<object>
                    {
                        Estatus = false,
                        Mensaje = "No se encontró la tarea.",
                        Error = resultado.error,
                        Data = null
                    });
                }

                return Ok(new ApiResponseDto<TareaDto>
                {
                    Estatus = true,
                    Mensaje = "Tarea obtenida correctamente.",
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

        [HttpPut("actualizarTarea/{id:int}")]
        public async Task<IActionResult> ActualizarTarea(int id, [FromBody] TareaActualizarDto dto)
        {
            try
            {
                int usuarioId = ObtenerUsuarioIdToken();
                var resultado = await _tareaService.ActualizarTareaAsync(usuarioId, id, dto);

                if (!resultado.ok)
                {
                    return BadRequest(new ApiResponseDto<object>
                    {
                        Estatus = false,
                        Mensaje = "No se pudo actualizar la tarea.",
                        Error = resultado.error,
                        Data = null
                    });
                }

                return Ok(new ApiResponseDto<TareaDto>
                {
                    Estatus = true,
                    Mensaje = "Tarea actualizada correctamente.",
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

        [HttpPut("completarTarea/{id:int}")]
        public async Task<IActionResult> CompletarTarea(int id)
        {
            try
            {
                int usuarioId = ObtenerUsuarioIdToken();
                var resultado = await _tareaService.CompletarTareaAsync(usuarioId, id);

                if (!resultado.ok)
                {
                    return BadRequest(new ApiResponseDto<object>
                    {
                        Estatus = false,
                        Mensaje = "No se pudo completar la tarea.",
                        Error = resultado.error,
                        Data = null
                    });
                }

                return Ok(new ApiResponseDto<object>
                {
                    Estatus = true,
                    Mensaje = "Tarea marcada como completada.",
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

        [HttpDelete("eliminarTarea/{id:int}")]
        public async Task<IActionResult> EliminarTarea(int id)
        {
            try
            {
                int usuarioId = ObtenerUsuarioIdToken();
                var resultado = await _tareaService.EliminarTareaAsync(usuarioId, id);

                if (!resultado.ok)
                {
                    return BadRequest(new ApiResponseDto<object>
                    {
                        Estatus = false,
                        Mensaje = "No se pudo eliminar la tarea.",
                        Error = resultado.error,
                        Data = null
                    });
                }

                return Ok(new ApiResponseDto<object>
                {
                    Estatus = true,
                    Mensaje = "Tarea eliminada correctamente.",
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

        private int ObtenerUsuarioIdToken()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                throw new Exception("No se encontró el identificador del usuario en el token.");

            return int.Parse(claim.Value);
        }
    }
}
