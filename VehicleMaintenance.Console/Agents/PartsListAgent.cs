using System.Text;
using System.Text.Json;
using Microsoft.SemanticKernel.ChatCompletion;
using VehicleMaintenance.Console.Models;

namespace VehicleMaintenance.Console.Agents;

public class PartsListAgent(IChatCompletionService chatService)
{
    private const string SystemPrompt = """
        Você é um especialista em peças automotivas.
        Dado uma lista de manutenções pendentes para um veículo, gere a lista de peças
        que o proprietário deve comprar antes de ir à oficina.

        Retorne SOMENTE um array JSON válido com este formato, sem texto adicional:
        [
          {
            "name": "nome da peça",
            "quantity": 1,
            "priority": "High|Medium|Low",
            "notes": "observações sobre a peça"
          }
        ]
        """;

    public async Task<List<Part>> GeneratePartsListAsync(
        List<MaintenanceSuggestion> suggestions,
        string vehicleName)
    {
        if (suggestions.Count == 0)
            return [];

        var context = BuildContext(suggestions, vehicleName);

        var history = new Microsoft.SemanticKernel.ChatCompletion.ChatHistory();
        history.AddSystemMessage(SystemPrompt);
        history.AddUserMessage(context);

        var response = await chatService.GetChatMessageContentAsync(history);
        var json = response.Content ?? "[]";

        return ParseParts(json);
    }

    private static string BuildContext(List<MaintenanceSuggestion> suggestions, string vehicleName)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Veículo: {vehicleName}");
        sb.AppendLine("Manutenções pendentes:");

        foreach (var s in suggestions)
            sb.AppendLine($"  [{s.Urgency}] {s.Type} — {s.Description}");

        return sb.ToString();
    }

    private static List<Part> ParseParts(string json)
    {
        try
        {
            json = JsonHelper.ExtractJsonArray(json);

            var items = JsonSerializer.Deserialize<List<JsonPartItem>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

            return items.Select(i => new Part
            {
                Name = i.Name,
                Quantity = i.Quantity,
                Priority = Enum.TryParse<Urgency>(i.Priority, true, out var p) ? p : Urgency.Medium,
                Notes = i.Notes
            }).ToList();
        }
        catch
        {
            return [];
        }
    }

    private sealed record JsonPartItem(
        string Name,
        int Quantity,
        string Priority,
        string Notes);
}
