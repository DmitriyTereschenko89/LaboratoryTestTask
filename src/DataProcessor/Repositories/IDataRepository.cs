using DataProcessor.Entities;
using System.Linq.Expressions;

namespace DataProcessor.Repositories;

public interface IDataRepository
{
    Task<bool> AddAsync(DeviceStatusEntity entity);
    Task<bool> AddRangeAsync(List<DeviceStatusEntity> items);

    Task<IEnumerable<DeviceStatusEntity>> GetModelsAsync(Expression<Func<DeviceStatusEntity, bool>> predicate = null);
}
