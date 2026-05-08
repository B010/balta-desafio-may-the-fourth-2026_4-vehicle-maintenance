namespace VehicleMaintenance.Console.Models;

public class VehicleRecord
{
    public DateTime Date { get; set; }
    public double Mileage { get; set; }
    public string Vehicle { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}
