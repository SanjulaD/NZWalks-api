using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class SqlWalkRepository: IWalkRepository
{
    private readonly NZWalksDbContext _dbContext;

    public SqlWalkRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Walk> CreateAsync(Walk walk)
    {
        await _dbContext.Walks.AddAsync(walk);
        await _dbContext.SaveChangesAsync();

        return walk;
    }
}