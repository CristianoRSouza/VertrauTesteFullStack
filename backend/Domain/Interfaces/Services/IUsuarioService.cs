using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces.Services;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> GetAllAsync(int page, int pageSize, string? filtro);
    Task<Usuario?> GetByIdAsync(long id);
    Task<Usuario> CreateAsync(Usuario usuario);
    Task<Usuario> UpdateAsync(long id, Usuario usuario);
    Task DeleteAsync(long id);
}
