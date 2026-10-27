using Microsoft.EntityFrameworkCore;

namespace GardenData;

public static class SupplierFunctions
{
	public static Task<List<Supplier>> GetAllSuppliersAsync(this GardenContext db)
	{
		return db.Suppliers.Include(s => s.Plants).ToListAsync();
	}

	public static Task<Supplier?> GetSupplierByIdAsync(this GardenContext db, int id)
	{
		return db.Suppliers.Include(s => s.Plants).FirstOrDefaultAsync(m => m.Id == id);
	}

	public static async Task CreateSupplierAsync(this GardenContext db, Supplier supplier)
	{
		db.Suppliers.Add(supplier);
		await db.SaveChangesAsync();
	}

	public static async Task UpdateSupplierAsync(this GardenContext db, Supplier supplier)
	{
		db.Attach(supplier).State = EntityState.Modified;
		try
		{
			await db.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!await db.SupplierExistsAsync(supplier.Id))
			{
				throw new InvalidOperationException($"Supplier with Id {supplier.Id} not found.");
			}
			else
			{
				throw;
			}
		}
	}

	public static async Task DeleteSupplierAsync(this GardenContext db, int id)
	{
		var supplier = await db.Suppliers.FindAsync(id);
		if (supplier != null)
		{
			db.Suppliers.Remove(supplier);
			await db.SaveChangesAsync();
		}
	}

	public static Task<bool> SupplierExistsAsync(this GardenContext db, int id)
	{
		return db.Suppliers.AnyAsync(e => e.Id == id);
	}
}
