using Microsoft.EntityFrameworkCore;

namespace GardenData;

public static class GuideFunctions
{
	public static Task<List<Guide>> GetAllGuidesAsync(this GardenContext db)
	{
		return db.Guides.ToListAsync();
	}

	public static Task<Guide?> GetGuideByIdAsync(this GardenContext db, int id)
	{
		return db.Guides.FirstOrDefaultAsync(m => m.Id == id);
	}

	public static async Task CreateGuideAsync(this GardenContext db, Guide guide)
	{
		db.Guides.Add(guide);
		await db.SaveChangesAsync();
	}

	public static async Task UpdateGuideAsync(this GardenContext db, Guide guide)
	{
		db.Attach(guide).State = EntityState.Modified;
		try
		{
			await db.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!await db.GuideExistsAsync(guide.Id))
			{
				throw new InvalidOperationException($"Guide with Id {guide.Id} not found.");
			}
			else
			{
				throw;
			}
		}
	}

	public static async Task DeleteGuideAsync(this GardenContext db, int id)
	{
		var guide = await db.Guides.FindAsync(id);
		if (guide != null)
		{
			db.Guides.Remove(guide);
			await db.SaveChangesAsync();
		}
	}

	public static Task<bool> GuideExistsAsync(this GardenContext db, int id)
	{
		return db.Guides.AnyAsync(e => e.Id == id);
	}
}
