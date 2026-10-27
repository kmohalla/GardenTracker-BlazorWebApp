using Microsoft.EntityFrameworkCore;

namespace GardenData;

public static class PlantFunctions
{
	public static Task<List<Plant>> GetAllPlantsAsync(this GardenContext db)
	{
		return db.Plants.Include(p => p.Supplier).ToListAsync();
	}

	public static Task<Plant?> GetPlantByIdAsync(this GardenContext db, int id)
	{
		return db.Plants.FirstOrDefaultAsync(m => m.Id == id);
	}

	public static async Task CreatePlantAsync(this GardenContext db, Plant plant)
	{
		db.Plants.Add(plant);
		await db.SaveChangesAsync();
	}

	public static async Task UpdatePlantAsync(this GardenContext db, Plant plant)
	{
		db.Attach(plant).State = EntityState.Modified;
		try
		{
			await db.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!await db.PlantExistsAsync(plant.Id))
			{
				throw new InvalidOperationException($"Plant with Id {plant.Id} not found.");
			}
			else
			{
				throw;
			}
		}
	}

	public static async Task DeletePlantAsync(this GardenContext db, int id)
	{
		var plant = await db.Plants.FindAsync(id);
		if (plant != null)
		{
			db.Plants.Remove(plant);
			await db.SaveChangesAsync();
		}
	}

	public static Task<bool> PlantExistsAsync(this GardenContext db, int id)
	{
		return db.Plants.AnyAsync(e => e.Id == id);
	}

	public static async Task<List<Supplier>> GetSuppliersForDropdownAsync(this GardenContext db)
	{
		var supplierQuery = from s in db.Suppliers
						 orderby s.Name
						 select s;
		return await supplierQuery.AsNoTracking().ToListAsync();
	}
}
