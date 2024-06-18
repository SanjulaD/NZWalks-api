using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public class SqlWalkRepository : IWalkRepository
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

    public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
        string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
    {
        var walks = _dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

        if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                walks = walks.Where(x => x.Name.Contains(filterQuery));

        if (string.IsNullOrWhiteSpace(sortBy) == false)
        {
            if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
            else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
        }

        var skipResult = (pageNumber - 1) * pageSize;

        return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
    }

    public async Task<Walk?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Walks
            .Include("Difficulty")
            .Include("Region")
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
    {
        var existingWalk = await _dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

        if (existingWalk == null) return null;

        Console.WriteLine(existingWalk);

        existingWalk.Description = walk.Description;
        existingWalk.Name = walk.Name;
        existingWalk.WalkImageUrl = walk.WalkImageUrl;
        existingWalk.LengthInKm = walk.LengthInKm;

        existingWalk.DifficultyId = walk.DifficultyId;
        existingWalk.RegionId = walk.RegionId;

        await _dbContext.SaveChangesAsync();
        return existingWalk;
    }

    public async Task<Walk?> DeleteAsync(Guid id)
    {
        var existingWalk = await _dbContext.Walks.FindAsync(id);

        if (existingWalk == null) return null;

        _dbContext.Walks.Remove(existingWalk);
        await _dbContext.SaveChangesAsync();

        return existingWalk;
    }
}