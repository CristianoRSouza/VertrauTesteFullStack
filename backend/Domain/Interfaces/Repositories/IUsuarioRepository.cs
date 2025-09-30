using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> GetAllAsync(int page = 1, int pageSize = 10, string? filtro = null);
    Task<Usuario?> GetByIdAsync(long id);
    Task<Usuario?> GetByEmailAsync(string email);
    Task<Usuario> CreateAsync(Usuario usuario);
    Task<Usuario> UpdateAsync(Usuario usuario);
    Task DeleteAsync(long id);
    Task<bool> EmailExistsAsync(string email, long? excludeId = null);
}
