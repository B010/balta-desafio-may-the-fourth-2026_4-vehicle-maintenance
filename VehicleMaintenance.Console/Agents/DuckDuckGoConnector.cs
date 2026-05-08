using System.Text.Json;
using Microsoft.SemanticKernel.Plugins.Web;

namespace VehicleMaintenance.Console.Agents;

#pragma warning disable SKEXP0050, SKEXP0054

public sealed class DuckDuckGoConnector : IWebSearchEngineConnector
{
    private static readonly HttpClient _client = new();

    static DuckDuckGoConnector()
    {
        _client.DefaultRequestHeaders.UserAgent.ParseAdd(
            "Mozilla/5.0 (compatible; VehicleMaintenance/1.0)");
    }

    public async Task<IEnumerable<T>> SearchAsync<T>(
        string query,
        int count = 1,
        int offset = 0,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var encoded = Uri.EscapeDataString(query);
            var url = $"https://api.duckduckgo.com/?q={encoded}&format=json&no_html=1&skip_disambig=1&t=VehicleMaintenance";

            var raw = await _client.GetStringAsync(url, cancellationToken);
            using var doc = JsonDocument.Parse(raw);

            var pages = new List<WebPage>();

            if (doc.RootElement.TryGetProperty("AbstractText", out var abs) &&
                abs.GetString() is { Length: > 0 } abstractText)
            {
                var source = doc.RootElement.TryGetProperty("AbstractSource", out var src)
                    ? src.GetString() ?? "DuckDuckGo" : "DuckDuckGo";
                var absUrl = doc.RootElement.TryGetProperty("AbstractURL", out var aUrl)
                    ? aUrl.GetString() ?? url : url;

                pages.Add(new WebPage { Name = source, Snippet = abstractText, Url = absUrl });
            }

            if (doc.RootElement.TryGetProperty("RelatedTopics", out var topics))
            {
                foreach (var topic in topics.EnumerateArray())
                {
                    if (pages.Count >= count) break;

                    if (topic.TryGetProperty("Text", out var text) &&
                        topic.TryGetProperty("FirstURL", out var firstUrl))
                    {
                        pages.Add(new WebPage
                        {
                            Name = "DuckDuckGo",
                            Snippet = text.GetString() ?? string.Empty,
                            Url = firstUrl.GetString() ?? string.Empty
                        });
                    }
                }
            }

            return CastPages<T>(pages);
        }
        catch
        {
            return [];
        }
    }

    private static IEnumerable<T> CastPages<T>(List<WebPage> pages)
    {
        if (typeof(T) == typeof(WebPage))
            return pages.Cast<T>();

        if (typeof(T) == typeof(string))
            return pages.Select(p => (T)(object)p.Snippet).ToList();

        return [];
    }
}

#pragma warning restore SKEXP0050, SKEXP0054
