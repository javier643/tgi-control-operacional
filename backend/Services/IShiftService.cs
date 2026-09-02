using TgiControl.Models;

namespace TgiControl.Services;

public interface IShiftService
{
    Task<IEnumerable<Shift>> GetAllAsync();
    Task<Shift?> GetByIdAsync(Guid id);
    Task<Shift> CreateAsync(Shift shift);
    Task<Shift> UpdateAsync(Guid id, Shift shift);
    Task<bool> DeleteAsync(Guid id);
    Task<IEnumerable<Shift>> GetByDateAsync(DateTime date);
}

public class ShiftService : IShiftService
{
    private readonly TgiDbContext _context;

    public ShiftService(TgiDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Shift>> GetAllAsync()
    {
        return await Task.FromResult(_context.Shifts.ToList());
    }

    public async Task<Shift?> GetByIdAsync(Guid id)
    {
        return await Task.FromResult(_context.Shifts.FirstOrDefault(s => s.Id == id));
    }

    public async Task<Shift> CreateAsync(Shift shift)
    {
        shift.Id = Guid.NewGuid();
        shift.CreatedAt = DateTime.UtcNow;
        _context.Shifts.Add(shift);
        await _context.SaveChangesAsync();
        return shift;
    }

    public async Task<Shift> UpdateAsync(Guid id, Shift shift)
    {
        var existing = await GetByIdAsync(id);
        if (existing == null) throw new KeyNotFoundException($"Shift {id} not found");

        existing.HandoverNotes = shift.HandoverNotes;
        existing.DeliveryTime = shift.DeliveryTime;
        existing.ReceiptTime = shift.ReceiptTime;
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var shift = await GetByIdAsync(id);
        if (shift == null) return false;

        _context.Shifts.Remove(shift);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Shift>> GetByDateAsync(DateTime date)
    {
        return await Task.FromResult(_context.Shifts.Where(s => s.Date.Date == date.Date).ToList());
    }
}
