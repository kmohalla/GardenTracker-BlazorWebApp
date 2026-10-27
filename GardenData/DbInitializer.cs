namespace GardenData
{
	// Creates and fills a small test database if the target database is empty.
	// Ported from KnittingData's DbInitializer, with three fixes that stopped the original from working:
	//   1. No explicit Id values: the Id columns are IDENTITY, so SQL Server rejects inserted Ids.
	//   2. Suppliers are actually added (the original built the brands array but never saved it).
	//   3. Parent rows are linked through navigation properties, so they are saved before the rows that use them.
	public static class DbInitializer
	{
		public static void Initialize(GardenContext context)
		{
			context.Database.EnsureCreated();
			// Look for any guides.
			if (context.Guides.Any())
			{
				return;   // DB has been seeded
			}

			var suppliers = new Supplier[]
			{
				new Supplier { Name = "Supplier 1", State = "Vermont" },
				new Supplier { Name = "Supplier 2", State = "Ohio" },
				new Supplier { Name = "Supplier 3", State = "Oregon" },
			};
			context.Suppliers.AddRange(suppliers);

			var plants = new Plant[]
			{
				new Plant { Name = "Plant 1", Family = "Nightshade", Type = "Vegetable", Color = "Red", Supplier = suppliers[0] },
				new Plant { Name = "Plant 2", Family = "Mint", Type = "Herb", Color = "Green", Supplier = suppliers[1] },
				new Plant { Name = "Plant 3", Family = "Aster", Type = "Flower", Color = "Yellow", Supplier = suppliers[2] },
			};
			context.Plants.AddRange(plants);

			var tools = new Tool[]
			{
				new Tool { WeightKg = 0.25M, Type = "Trowel", Material = "Stainless Steel", LengthCm = 30 },
				new Tool { WeightKg = 1.40M, Type = "Hoe", Material = "Steel/Ash", LengthCm = 150 },
				new Tool { WeightKg = 0.30M, Type = "Pruner", Material = "Carbon Steel", LengthCm = 20 },
			};
			context.Tools.AddRange(tools);

			var guides = new Guide[]
			{
				new Guide { Name = "Guide 1", Author = "Author 1", Season = "Spring", Difficulty = "Beginner" },
				new Guide { Name = "Guide 2", Author = "Author 2", Season = "Summer", Difficulty = "Intermediate" },
				new Guide { Name = "Guide 3", Author = "Author 3", Season = "Fall", Difficulty = "Advanced" },
			};
			context.Guides.AddRange(guides);

			var beds = new GardenBed[]
			{
				new GardenBed { Name = "Bed 1", Status = "Growing", PlantedDate = DateTime.Parse("2026-04-01"), Guide = guides[0], Plant = plants[0], Tool = tools[0] },
				new GardenBed { Name = "Bed 2", Status = "Harvested", PlantedDate = DateTime.Parse("2026-05-01"), Guide = guides[1], Plant = plants[1], Tool = tools[1] },
				new GardenBed { Name = "Bed 3", Status = "Planned", PlantedDate = DateTime.Parse("2027-03-01"), Guide = guides[2], Plant = plants[2], Tool = tools[2] },
			};
			context.GardenBeds.AddRange(beds);

			// One save inserts everything in dependency order.
			context.SaveChanges();
		}
	}
}
