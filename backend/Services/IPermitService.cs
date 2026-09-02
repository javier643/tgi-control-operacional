using TgiControl.Models;

namespace TgiControl.Services;

public interface IPermitService
{
    Task<IEnumerable<Permit>> GetAllAsync();
    Task<Permit?> GetByIdAsync(Guid id);
    Task<Permit> CreateAsync(Permit permit);
    Task<Permit> UpdateAsync(Guid id, Permit permit);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<Permit>> GetByStatusAsync(PermitStatus status);
}

public class PermitService : IPermitService
{
    private readonly TgiDbContext _context;

    public PermitService(TgiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Permit>> GetAllAsync()
    {
        return await Task.FromResult(_context.Permits.ToList());
    }

    public async Task<Permit?> GetByIdAsync(Guid id)
    {
        return await Task.FromResult(_context.Permits.FirstOrDefault(p => p.Id == id));
    }

    public async Task<Permit> CreateAsync(Permit permit)
    {
        permit.Id = Guid.NewGuid();
        permit.CreatedAt = DateTime.UtcNow;
        _context.Permits.Add(permit);
        await _context.SaveChangesAsync();
        return permit;
    }

    public async Task<Permit> UpdateAsync(Guid id, Permit permit)
    {
        var existing = await GetByIdAsync(id);
        if (existing == null) throw new KeyNotFoundException($"Permit {id} not found");

        existing.Description = permit.Description;
        existing.Status = permit.Status;
        existing.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var permit = await GetByIdAsync(id);
        if (permit == null) return false;

        _context.Permits.Remove(permit);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Permit>> GetByStatusAsync(PermitStatus status)
    {
        return await Task.FromResult(_context.Permits.Where(p => p.Status == status).ToList());
    }
}
