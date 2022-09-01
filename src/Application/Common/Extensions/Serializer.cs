using System.Text.Json;

namespace Application.Common.Extensions;

public static class Serializer
{
    private static JsonSerializerOptions _option = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    public static string Serialize(this object data)
    {
        if (data == default) throw new ArgumentNullException();

        return JsonSerializer.Serialize(data, _option);
    }

    public static T Deserialize<T>(this string data)
    {
        if (string.IsNullOrWhiteSpace(data)) throw new ArgumentNullException();

        return JsonSerializer.Deserialize<T>(data, _option)!;
    }
}
