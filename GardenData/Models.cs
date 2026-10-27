namespace GardenData;

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string State { get; set; } = "";
    public List<Plant> Plants { get; set; } = new();
}

public class Plant
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Family { get; set; } = "";
    public string Type { get; set; } = "";
    public string Color { get; set; } = "";
    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
}

public class Tool
{
    public int Id { get; set; }
    public decimal WeightKg { get; set; }
    public string Type { get; set; } = "";
    public string Material { get; set; } = "";
    public int LengthCm { get; set; }
}

public class Guide
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Author { get; set; } = "";
    public string Season { get; set; } = "";
    public string Difficulty { get; set; } = "";
}

public class GardenBed
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Status { get; set; } = "";
    public DateTime PlantedDate { get; set; }
    public int? GuideId { get; set; }
    public Guide? Guide { get; set; }
    public int? PlantId { get; set; }
    public Plant? Plant { get; set; }
    public int? ToolId { get; set; }
    public Tool? Tool { get; set; }
}
