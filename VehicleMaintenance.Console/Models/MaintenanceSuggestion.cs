namespace VehicleMaintenance.Console.Models;

public enum Urgency { High, Medium, Low }

public class MaintenanceSuggestion
{
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Urgency Urgency { get; set; }
    public double LastDoneAtMileage { get; set; }
    public double CurrentMileage { get; set; }
    public double OverdueMileage => CurrentMileage - LastDoneAtMileage;
}
