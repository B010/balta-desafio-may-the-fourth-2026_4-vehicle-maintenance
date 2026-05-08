using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Plugins.Web;
using VehicleMaintenance.Console.Agents;
using VehicleMaintenance.Console.Models;

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true)
    .AddUserSecrets<Program>()
    .AddEnvironmentVariables()
    .Build();

var openAiKey = config["OpenAI:ApiKey"]
    ?? throw new InvalidOperationException("OpenAI:ApiKey não configurada. Defina via: dotnet user-secrets set \"OpenAI:ApiKey\" \"sk-...\"");

var kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion("gpt-4o", openAiKey)
    .Build();

#pragma warning disable SKEXP0050, SKEXP0054
kernel.ImportPluginFromObject(
    new WebSearchEnginePlugin(new DuckDuckGoConnector()),
    "WebSearch");
#pragma warning restore SKEXP0050, SKEXP0054

var chatService = kernel.GetRequiredService<IChatCompletionService>();

PrintHeader();

// Coleta dados do veículo do usuário
var vehicleName = AskVehicleName();
var currentMileage = AskCurrentMileage();

Console.WriteLine();

// Agente 1: tenta carregar histórico do CSV (opcional)
var csvPath = args.Length > 0 ? args[0] : Path.Combine(AppContext.BaseDirectory, "Data", "historico.csv");
var csvAgent = new CsvReaderAgent();
List<VehicleRecord> records;

try
{
    records = csvAgent.ReadRecords(csvPath);
    // Sobrescreve o nome do veículo e injeta o km atual informado pelo usuário
    foreach (var r in records)
        r.Vehicle = vehicleName;

    // Garante que o km atual esteja presente como último ponto de dados
    if (records.Max(r => r.Mileage) < currentMileage)
        records.Add(new VehicleRecord
        {
            Date = DateTime.Today,
            Mileage = currentMileage,
            Vehicle = vehicleName,
            Notes = "Quilometragem atual informada pelo usuário"
        });

    Console.WriteLine($"[OK] Histórico carregado: {records.Count} registros");
}
catch
{
    // Sem CSV — cria um registro único com os dados informados
    records = [new VehicleRecord
    {
        Date = DateTime.Today,
        Mileage = currentMileage,
        Vehicle = vehicleName,
        Notes = "Quilometragem atual informada pelo usuário"
    }];

    Console.WriteLine("[INFO] Nenhum histórico CSV encontrado. Usando apenas a quilometragem informada.");
}

Console.WriteLine($"[OK] Veículo: {vehicleName} | Quilometragem: {currentMileage:N0} km\n");

// Agente 2: análise de manutenção
Console.WriteLine("Analisando manutenções pendentes...");
var maintenanceAgent = new MaintenanceAgent(kernel);
var suggestions = await maintenanceAgent.AnalyzeAsync(records, currentMileage);

PrintMaintenanceSuggestions(suggestions);

if (suggestions.Count == 0)
{
    Console.WriteLine("Nenhuma manutenção pendente identificada.");
    return;
}

// Agente 3: lista de peças
Console.WriteLine("\nGerando lista de peças...");
var partsAgent = new PartsListAgent(chatService);
var parts = await partsAgent.GeneratePartsListAsync(suggestions, vehicleName);

PrintPartsList(parts);

Console.WriteLine("\n[Análise concluída]");

// ── Helpers de input ──────────────────────────────────────────────────────────

static string AskVehicleName()
{
    string? value;
    do
    {
        Console.Write("Modelo do seu carro (ex: Honda Civic 2022): ");
        value = Console.ReadLine()?.Trim();
    }
    while (string.IsNullOrWhiteSpace(value));

    return value;
}

static double AskCurrentMileage()
{
    while (true)
    {
        Console.Write("Quilometragem atual (km): ");
        var input = Console.ReadLine()?.Trim().Replace(".", "").Replace(",", "");
        if (double.TryParse(input, out var mileage) && mileage >= 0)
            return mileage;

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("  Valor inválido. Digite apenas números (ex: 52000).");
        Console.ResetColor();
    }
}

// ── Helpers de output ─────────────────────────────────────────────────────────

static void PrintHeader()
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("╔══════════════════════════════════════════════╗");
    Console.WriteLine("║       VEHICLE MAINTENANCE AI ASSISTANT       ║");
    Console.WriteLine("║          May The Fourth 2026 - Desafio 4     ║");
    Console.WriteLine("╚══════════════════════════════════════════════╝");
    Console.ResetColor();
    Console.WriteLine();
}

static void PrintMaintenanceSuggestions(List<MaintenanceSuggestion> suggestions)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("═══ MANUTENÇÕES PENDENTES ═══");
    Console.ResetColor();

    foreach (var s in suggestions.OrderBy(s => s.Urgency))
    {
        var color = s.Urgency switch
        {
            Urgency.High   => ConsoleColor.Red,
            Urgency.Medium => ConsoleColor.Yellow,
            _              => ConsoleColor.Green
        };

        Console.ForegroundColor = color;
        Console.Write($"[{s.Urgency.ToString().ToUpper(),6}] ");
        Console.ResetColor();
        Console.WriteLine(s.Type);
        Console.WriteLine($"          {s.Description}");
    }
}

static void PrintPartsList(List<Part> parts)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine("═══ LISTA DE PEÇAS PARA COMPRAR ═══");
    Console.ResetColor();

    foreach (var p in parts.OrderBy(p => p.Priority))
    {
        var color = p.Priority switch
        {
            Urgency.High   => ConsoleColor.Red,
            Urgency.Medium => ConsoleColor.Yellow,
            _              => ConsoleColor.Green
        };

        Console.ForegroundColor = color;
        Console.Write($"[{p.Priority.ToString().ToUpper(),6}] ");
        Console.ResetColor();
        Console.Write($"{p.Name} x{p.Quantity}");

        if (!string.IsNullOrWhiteSpace(p.Notes))
            Console.Write($"  — {p.Notes}");

        Console.WriteLine();
    }
}
