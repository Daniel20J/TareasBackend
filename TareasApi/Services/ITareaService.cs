using TareasApi.DTOs;

namespace TareasApi.Services
{
    public interface ITareaService
    {
        Task<(bool ok, string error, TareaDto? data)> CrearTareaAsync(int usuarioId, TareaCrearDto dto);
        Task<(bool ok, string error, List<TareaDto>? data)> GetTareasAsync(int usuarioId);
        Task<(bool ok, string error, TareaDto? data)> GetTareasIdAsync(int usuarioId, int tareaId);
        Task<(bool ok, string error, TareaDto? data)> ActualizarTareaAsync(int usuarioId, int tareaId, TareaActualizarDto dto);
        Task<(bool ok, string error)> CompletarTareaAsync(int usuarioId, int tareaId);
        Task<(bool ok, string error)> EliminarTareaAsync(int usuarioId, int tareaId);
    }
}
