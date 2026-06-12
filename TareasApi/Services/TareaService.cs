using Microsoft.EntityFrameworkCore;
using TareasApi.Data;
using TareasApi.DTOs;
using TareasApi.Models;

namespace TareasApi.Services
{
    public class TareaService : ITareaService
    {
        private readonly ApplicationDbContext _db;

        public TareaService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<(bool ok, string error, TareaDto? data)> CrearTareaAsync(int usuarioId, TareaCrearDto dto)
        {
            try
            {
                var titulo = dto.Titulo?.Trim() ?? "";
                var descripcion = dto.Descripcion?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(titulo))
                    return (false, "El título es obligatorio.", null);

                var tarea = new Tarea
                {
                    Titulo = titulo,
                    Descripcion = descripcion,
                    Completada = false,
                    FechaCreacion = DateTime.Now,
                    UsuarioId = usuarioId
                };

                _db.Tareas.Add(tarea);
                await _db.SaveChangesAsync();

                var respuesta = new TareaDto
                {
                    Id = tarea.Id,
                    Titulo = tarea.Titulo,
                    Descripcion = tarea.Descripcion,
                    Completada = tarea.Completada,
                    FechaCreacion = tarea.FechaCreacion,
                    UsuarioId = tarea.UsuarioId
                };

                return (true, "", respuesta);
            }
            catch (Exception ex)
            {
                return (false, $"Ocurrió un error al crear la tarea: {ex.Message}", null);
            }
        }

        public async Task<(bool ok, string error, List<TareaDto>? data)> GetTareasAsync(int usuarioId)
        {
            try
            {
                var tareas = await _db.Tareas
                    .Where(t => t.UsuarioId == usuarioId)
                    .OrderByDescending(t => t.Id)
                    .Select(t => new TareaDto
                    {
                        Id = t.Id,
                        Titulo = t.Titulo,
                        Descripcion = t.Descripcion,
                        Completada = t.Completada,
                        FechaCreacion = t.FechaCreacion,
                        UsuarioId = t.UsuarioId
                    })
                    .ToListAsync();

                return (true, "", tareas);
            }
            catch (Exception ex)
            {
                return (false, $"Ocurrió un error al listar las tareas: {ex.Message}", null);
            }
        }

        public async Task<(bool ok, string error, TareaDto? data)> GetTareasIdAsync(int usuarioId, int tareaId)
        {
            try
            {
                var tarea = await _db.Tareas
                    .Where(t => t.UsuarioId == usuarioId && t.Id == tareaId)
                    .Select(t => new TareaDto
                    {
                        Id = t.Id,
                        Titulo = t.Titulo,
                        Descripcion = t.Descripcion,
                        Completada = t.Completada,
                        FechaCreacion = t.FechaCreacion,
                        UsuarioId = t.UsuarioId
                    })
                    .FirstOrDefaultAsync();

                if (tarea == null)
                    return (false, "La tarea no existe.", null);

                return (true, "", tarea);
            }
            catch (Exception ex)
            {
                return (false, $"Ocurrió un error al obtener la tarea: {ex.Message}", null);
            }
        }

        public async Task<(bool ok, string error, TareaDto? data)> ActualizarTareaAsync(int usuarioId, int tareaId, TareaActualizarDto dto)
        {
            try
            {
                var titulo = dto.Titulo?.Trim() ?? "";
                var descripcion = dto.Descripcion?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(titulo))
                    return (false, "El título es obligatorio.", null);

                var tarea = await _db.Tareas.FirstOrDefaultAsync(t => t.UsuarioId == usuarioId && t.Id == tareaId);

                if (tarea == null)
                    return (false, "La tarea no existe.", null);

                tarea.Titulo = titulo;
                tarea.Descripcion = descripcion;

                await _db.SaveChangesAsync();

                var respuesta = new TareaDto
                {
                    Id = tarea.Id,
                    Titulo = tarea.Titulo,
                    Descripcion = tarea.Descripcion,
                    Completada = tarea.Completada,
                    FechaCreacion = tarea.FechaCreacion,
                    UsuarioId = tarea.UsuarioId
                };

                return (true, "", respuesta);
            }
            catch (Exception ex)
            {
                return (false, $"Ocurrió un error al actualizar la tarea: {ex.Message}", null);
            }
        }

        public async Task<(bool ok, string error)> CompletarTareaAsync(int usuarioId, int tareaId)
        {
            try
            {
                var tarea = await _db.Tareas.FirstOrDefaultAsync(t => t.UsuarioId == usuarioId && t.Id == tareaId);

                if (tarea == null)
                    return (false, "La tarea no existe.");

                tarea.Completada = true;
                await _db.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, $"Ocurrió un error al completar la tarea: {ex.Message}");
            }
        }

        public async Task<(bool ok, string error)> EliminarTareaAsync(int usuarioId, int tareaId)
        {
            try
            {
                var tarea = await _db.Tareas.FirstOrDefaultAsync(t => t.UsuarioId == usuarioId && t.Id == tareaId);

                if (tarea == null)
                    return (false, "La tarea no existe.");

                _db.Tareas.Remove(tarea);
                await _db.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, $"Ocurrió un error al eliminar la tarea: {ex.Message}");
            }
        }
    }
}
