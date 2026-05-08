namespace VehicleMaintenance.Console.Agents;

internal static class JsonHelper
{
    // GPT-4o sometimes wraps the response in ```json ... ``` fences.
    // This method strips fences and extracts the first JSON array found.
    internal static string ExtractJsonArray(string content)
    {
        content = content.Trim();

        if (content.Contains("```"))
        {
            var fenceStart = content.IndexOf("```");
            var lineBreak = content.IndexOf('\n', fenceStart);
            var fenceEnd = content.LastIndexOf("```");

            if (lineBreak >= 0 && fenceEnd > lineBreak)
                content = content[(lineBreak + 1)..fenceEnd].Trim();
        }

        var start = content.IndexOf('[');
        var end = content.LastIndexOf(']');

        return start >= 0 && end > start
            ? content[start..(end + 1)]
            : content;
    }
}
