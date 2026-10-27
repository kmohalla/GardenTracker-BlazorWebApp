using Microsoft.EntityFrameworkCore;

namespace GardenData;

public static class ToolFunctions
{
	public static Task<List<Tool>> GetAllToolsAsync(this GardenContext db)
	{
		return db.Tools.ToListAsync();
	}

	public static Task<Tool?> GetToolByIdAsync(this GardenContext db, int id)
	{
		return db.Tools.FirstOrDefaultAsync(m => m.Id == id);
	}

	public static async Task CreateToolAsync(this GardenContext db, Tool tool)
	{
		db.Tools.Add(tool);
		await db.SaveChangesAsync();
	}

	public static async Task UpdateToolAsync(this GardenContext db, Tool tool)
	{
		db.Attach(tool).State = EntityState.Modified;
		try
		{
			await db.SaveChangesAsync();
		}
		catch (DbUpdateConcurrencyException)
		{
			if (!await db.ToolExistsAsync(tool.Id))
			{
				throw new InvalidOperationException($"Tool with Id {tool.Id} not found.");
			}
			else
			{
				throw;
			}
		}
	}

	public static async Task DeleteToolAsync(this GardenContext db, int id)
	{
		var tool = await db.Tools.FindAsync(id);
		if (tool != null)
		{
			db.Tools.Remove(tool);
			await db.SaveChangesAsync();
		}
	}

	public static Task<bool> ToolExistsAsync(this GardenContext db, int id)
	{
		return db.Tools.AnyAsync(e => e.Id == id);
	}
}
