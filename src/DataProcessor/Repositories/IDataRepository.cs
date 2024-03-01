using DataProcessor.Entities;
using System.Linq.Expressions;

namespace DataProcessor.Repositories;

public interface IDataRepository
{
    Task<bool> AddAsync(ModelEntity entity);
    Task<bool> AddRangeAsync(List<ModelEntity> items);

    Task<IEnumerable<ModelEntity>> GetModelsAsync(Expression<Func<ModelEntity, bool>> predicate = null);
}
