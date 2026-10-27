using Microsoft.EntityFrameworkCore;

namespace GardenData;

public class GardenContext : DbContext
{
    public GardenContext(DbContextOptions<GardenContext> options) : base(options) { }

    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Plant> Plants => Set<Plant>();
    public DbSet<Tool> Tools => Set<Tool>();
    public DbSet<Guide> Guides => Set<Guide>();
    public DbSet<GardenBed> GardenBeds => Set<GardenBed>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Tool>().Property(t => t.WeightKg).HasPrecision(4, 2);
        b.Entity<GardenBed>().Property(g => g.PlantedDate).HasColumnType("date");
    }
}
