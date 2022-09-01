using Application.Common.Models;

namespace Application.Common.Interfaces;
public interface IBaseDAO<T> where T : BaseModel
{
    Task<long> InsertAsync(T model, CancellationToken cancellationToken);
    Task<T> FindAsync(long id, CancellationToken cancellationToken);
    Task UpdateAsync(T model, CancellationToken cancellationToken);
}
