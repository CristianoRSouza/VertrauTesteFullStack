using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IAuthService
{
    Task<string?> LoginAsync(string email, string senha);
    string GenerateToken(Usuario usuario);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}
