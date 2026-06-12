using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TareasApi.Data;
using TareasApi.DTOs;
using TareasApi.Models;

namespace TareasApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;

        public AuthService(ApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<(bool ok, string error)> RegistrarAsync(RegistroDto dto)
        {
            try
            {
                var nombreCompleto = dto.NombreCompleto?.Trim() ?? "";
                var usuarioLogin = dto.UsuarioLogin?.Trim() ?? "";
                var password = dto.Password?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(nombreCompleto))
                    return (false, "El nombre completo es obligatorio.");

                if (string.IsNullOrWhiteSpace(usuarioLogin))
                    return (false, "El usuarioLogin es obligatorio.");

                if (string.IsNullOrWhiteSpace(password))
                    return (false, "La contraseña es obligatoria.");

                var existe = await _db.Usuarios.AnyAsync(u => u.UsuarioLogin == usuarioLogin);
                if (existe)
                    return (false, "El usuario ya existe.");

                var usuario = new Usuario
                {
                    NombreCompleto = nombreCompleto,
                    UsuarioLogin = usuarioLogin,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
                };

                _db.Usuarios.Add(usuario);
                await _db.SaveChangesAsync();

                return (true, "");
            }
            catch (Exception ex)
            {
                return (false, $"Ocurrió un error al registrar el usuario: {ex.Message}");
            }
        }

        public async Task<(bool ok, string error, AuthResponseDto data)> LoginAsync(LoginDto dto)
        {
            try
            {
                var usuarioLogin = dto.UsuarioLogin?.Trim() ?? "";
                var password = dto.Password?.Trim() ?? "";

                if (string.IsNullOrWhiteSpace(usuarioLogin))
                    return (false, "El usuarioLogin es obligatorio.", null);

                if (string.IsNullOrWhiteSpace(password))
                    return (false, "La contraseña es obligatoria.", null);

                var usuario = await _db.Usuarios.FirstOrDefaultAsync(u => u.UsuarioLogin == usuarioLogin);

                if (usuario == null)
                    return (false, "Usuario o contraseña incorrectos.", null);

                var passwordValido = BCrypt.Net.BCrypt.Verify(password, usuario.PasswordHash);
                if (!passwordValido)
                    return (false, "Usuario o contraseña incorrectos.", null);

                var token = GenerarToken(usuario, out DateTime expira);

                var respuesta = new AuthResponseDto
                {
                    Token = token,
                    Expira = expira,
                    UsuarioLogin = usuario.UsuarioLogin
                };

                return (true, "", respuesta);
            }
            catch (Exception ex)
            {
                return (false, $"Ocurrió un error al iniciar sesión: {ex.Message}", null);
            }
        }

        private string GenerarToken(Usuario usuario, out DateTime expira)
        {
            var jwtKey = _config["Jwt:Key"]!;
            var jwtIssuer = _config["Jwt:Issuer"]!;
            var jwtAudience = _config["Jwt:Audience"]!;
            var jwtExpireMinutes = int.Parse(_config["Jwt:ExpireMinutes"]!);

            expira = DateTime.UtcNow.AddMinutes(jwtExpireMinutes);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.UsuarioLogin)
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: expira,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}