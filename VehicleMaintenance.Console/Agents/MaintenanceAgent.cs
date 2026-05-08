using System.Text;
using System.Text.Json;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using VehicleMaintenance.Console.Models;

namespace VehicleMaintenance.Console.Agents;

public class MaintenanceAgent(Kernel kernel)
{
    private const string SystemPrompt = """
        Você é um especialista em manutenção automotiva.
        Sua tarefa é identificar quais manutenções estão pendentes ou próximas do vencimento
        para o veículo informado, com base na quilometragem atual.

        PASSO 1 — Tente buscar na web as recomendações OFICIAIS do fabricante para o modelo
        exato, usando termos como "[modelo] maintenance schedule km" ou
        "[modelo] tabela revisão quilometragem".

        PASSO 2 — Se a busca falhar ou não retornar resultados úteis, use os intervalos
        padrão abaixo como fallback:
        - Troca de Óleo:      a cada  5.000 km
        - Rodízio de Pneus:   a cada 10.000 km
        - Filtro de Ar:       a cada 15.000 km
        - Revisão Geral:      a cada 10.000 km
        - Velas de Ignição:   a cada 30.000 km
        - Troca de Pneus:     a cada 40.000 km

        Retorne SOMENTE um array JSON válido com este formato, sem texto adicional:
        [
          {
            "type": "nome da manutenção",
            "description": "descrição detalhada indicando se veio da busca ou do padrão",
            "urgency": "High|Medium|Low",
            "lastDoneAtMileage": 0,
            "currentMileage": 0
          }
        ]

        Urgência: High = passou do intervalo ou está nos últimos 10%, Medium = dentro de 20%, Low = preventivo.
        """;

    public async Task<List<MaintenanceSuggestion>> AnalyzeAsync(
        List<VehicleRecord> records,
        double currentMileage)
    {
        var chatService = kernel.GetRequiredService<IChatCompletionService>();

        var history = new ChatHistory();
        history.AddSystemMessage(SystemPrompt);
        history.AddUserMessage(BuildContext(records, currentMileage));

        var settings = new OpenAIPromptExecutionSettings
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        var response = await chatService.GetChatMessageContentAsync(history, settings, kernel);
        var json = response.Content ?? "[]";

        return ParseSuggestions(json, currentMileage);
    }

    private static string BuildContext(List<VehicleRecord> records, double currentMileage)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"Veículo: {records.FirstOrDefault()?.Vehicle ?? "Desconhecido"}");
        sb.AppendLine($"Quilometragem atual: {currentMileage:N0} km");
        sb.AppendLine();
        sb.AppendLine("Histórico de registros (últimos 10):");

        foreach (var r in records.TakeLast(10))
            sb.AppendLine($"  {r.Date:dd/MM/yyyy} — {r.Mileage:N0} km — {r.Notes}");

        return sb.ToString();
    }

    private static List<MaintenanceSuggestion> ParseSuggestions(string json, double currentMileage)
    {
        try
        {
            json = JsonHelper.ExtractJsonArray(json);

            var items = JsonSerializer.Deserialize<List<JsonMaintenanceItem>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? [];

            return items.Select(i => new MaintenanceSuggestion
            {
                Type = i.Type,
                Description = i.Description,
                Urgency = Enum.TryParse<Urgency>(i.Urgency, true, out var u) ? u : Urgency.Medium,
                LastDoneAtMileage = i.LastDoneAtMileage,
                CurrentMileage = currentMileage
            }).ToList();
        }
        catch
        {
            return [];
        }
    }

    private sealed record JsonMaintenanceItem(
        string Type,
        string Description,
        string Urgency,
        double LastDoneAtMileage,
        double CurrentMileage);
}
