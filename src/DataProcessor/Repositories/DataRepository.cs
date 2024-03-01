using DataProcessor.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataProcessor.Repositories;

public class DataRepository : IDataRepository
{
    private readonly DataContext _dbContext;
    public DataRepository(DataContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddAsync(ModelEntity entity)
    {
        var modelEntity = _dbContext.Models.Where(model => model.ModuleCategoryID == entity.ModuleCategoryID).FirstOrDefault();
        if (modelEntity is null)
        {
            _dbContext.Models.Add(entity);
        }
        else
        {
            modelEntity.ModuleState = entity.ModuleState;
            _dbContext.Entry(modelEntity).State = EntityState.Modified;
        }
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AddRangeAsync(List<ModelEntity> items)
    {
        foreach (var item in items)
        {
            await AddAsync(item);
        }
        return true;
    }

    public async Task<IEnumerable<ModelEntity>> GetModelsAsync(Expression<Func<ModelEntity, bool>> predicate = null)
    {
        return await _dbContext.Models.Where(predicate).ToListAsync();
    }
}
