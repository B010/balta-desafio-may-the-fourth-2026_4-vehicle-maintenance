namespace VehicleMaintenance.Console.Models;

public class Part
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Urgency Priority { get; set; }
    public string Notes { get; set; } = string.Empty;
}
