using TareasApi.DTOs;

namespace TareasApi.Services
{
    public interface IAuthService
    {
        Task<(bool ok, string error)> RegistrarAsync(RegistroDto dto);
        Task<(bool ok, string error, AuthResponseDto? data)> LoginAsync(LoginDto dto);
    }
}