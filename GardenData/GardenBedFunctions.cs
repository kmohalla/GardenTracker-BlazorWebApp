using Microsoft.EntityFrameworkCore;

namespace GardenData;

public static class GardenBedFunctions
{
	public static Task<List<GardenBed>> GetAllGardenBedsAsync(this GardenContext db)
	{
		return db.GardenBeds.Include(b => b.Guide).Include(b => b.Plant).Include(b => b.Tool).ToListAsync();
	}

	public static Task<GardenBed?> GetGardenBedByIdAsync(this GardenContext db, int id)
	{
		return db.GardenBeds.Include(b => b.Guide).Include(b => b.Plant).Include(b => b.Tool).FirstOrDefaultAsync(m => m.Id == id);
	}

	public static async Task CreateGardenBedAsync(this GardenContext db, GardenBed bed)
	{
		db.GardenBeds.Add(bed);
		await db.SaveChangesAsync();
	}

	public static async Task UpdateGardenBedAsync(this GardenContext db, GardenBed bed)
	{
		db.Attach(bed).State = EntityState.Modified;
		try
		{
			await db.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!await db.GardenBedExistsAsync(bed.Id))
			{
				throw new InvalidOperationException($"GardenBed with Id {bed.Id} not found.");
			}
			else
			{
				throw;
			}
		}
	}

	public static async Task DeleteGardenBedAsync(this GardenContext db, int id)
	{
		var bed = await db.GardenBeds.FindAsync(id);
		if (bed != null)
		{
			db.GardenBeds.Remove(bed);
			await db.SaveChangesAsync();
		}
	}

	public static Task<bool> GardenBedExistsAsync(this GardenContext db, int id)
	{
		return db.GardenBeds.AnyAsync(e => e.Id == id);
	}

	public static async Task<List<Guide>> GetGuidesForDropdownAsync(this GardenContext db)
	{
		var guideQuery = from g in db.Guides
						   orderby g.Name
						   select g;
		return await guideQuery.AsNoTracking().ToListAsync();
	}

	public static async Task<List<Plant>> GetPlantsForDropdownAsync(this GardenContext db)
	{
		var plantQuery = from p in db.Plants
						orderby p.Name
						select p;
		return await plantQuery.AsNoTracking().ToListAsync();
	}

	public static async Task<List<Tool>> GetToolsForDropdownAsync(this GardenContext db)
	{
		var toolQuery = from t in db.Tools
						  orderby t.Type, t.LengthCm
						  select t;
		return await toolQuery.AsNoTracking().ToListAsync();
	}
}
