using DemoLibrary.Infraestructure.DataAccess.Context;

namespace DemoLibrary.Application.DataAccess;

public interface IRepository<T> where T : class
{
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
}

public interface ICommonRepository<T> : IRepository<T> where T : class
{
    Task<T> GetByIdAsync(long id);

    T GetByIdSync(long id);
    Task<T> DeleteAsync(long id);

    public Task<T> GetByProperty(string propertyName, string value);
}

public interface ITokenRepository<T> : IRepository<T> where T : class
{
    Task<T> GetByIdAsync(string id);

    Task<T> DeleteAsync(string id);
    
    Task <List<T>> GetAllExpiredAsync();
}