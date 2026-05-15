using System.Text.Json;

namespace ECommerceAdminPanel.Helper;

public static class JsonHelper
{
    public static List<string> ToList(string? json)
    {
        if (string.IsNullOrEmpty(json)) return new List<string>();
        return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>();
    }

    public static string? ToJson(List<string>? list)
    {
        if (list == null || !list.Any()) return null;
        return JsonSerializer.Serialize(list);
    }
}