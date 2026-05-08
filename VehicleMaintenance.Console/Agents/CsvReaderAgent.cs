using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using VehicleMaintenance.Console.Models;

namespace VehicleMaintenance.Console.Agents;

public class CsvReaderAgent
{
    public List<VehicleRecord> ReadRecords(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Arquivo CSV não encontrado: {filePath}");

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null
        };

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);

        csv.Context.RegisterClassMap<VehicleRecordMap>();

        var records = csv.GetRecords<VehicleRecord>().ToList();

        return [.. records.OrderBy(r => r.Date)];
    }
}

public sealed class VehicleRecordMap : ClassMap<VehicleRecord>
{
    public VehicleRecordMap()
    {
        Map(m => m.Date).Name("Data");
        Map(m => m.Mileage).Name("Quilometragem");
        Map(m => m.Vehicle).Name("Veiculo");
        Map(m => m.Notes).Name("Observacoes").Optional();
    }
}
