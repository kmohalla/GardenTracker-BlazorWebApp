using Microsoft.EntityFrameworkCore;

namespace GardenData;

public static class PlantSearch
{
    // Search plants by name, family, or color (any field may be blank).
    public static Task<List<Plant>> SearchAsync(this GardenContext db,
        string? name, string? family, string? color)
    {
        var q = db.Plants.Include(p => p.Supplier).AsQueryable();
        if (!string.IsNullOrWhiteSpace(name))   q = q.Where(p => p.Name.Contains(name));
        if (!string.IsNullOrWhiteSpace(family)) q = q.Where(p => p.Family.Contains(family));
        if (!string.IsNullOrWhiteSpace(color))  q = q.Where(p => p.Color.Contains(color));
        return q.OrderBy(p => p.Name).ToListAsync();
    }
}
