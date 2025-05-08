
namespace ParkingApi.Application.Services;

public class ConverterCsvToList : IConverterCsvToList
{
    public async Task<List<T>> Converter<T>(IFormFile file) where T : new()
    {
        using var reader = new StreamReader(file.OpenReadStream());
        var result = new List<T>();
        string? line;
        int lineNumber = 0;
        string[]? headers = null;


        var properties = typeof(T)
           .GetProperties(BindingFlags.Public | BindingFlags.Instance)
           .Where(p => p.CanWrite)
           .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

        while ((line = await reader.ReadLineAsync()) != null)
        {
            lineNumber++;
            var values = line.Split(',');

            if (lineNumber == 1)
            {
                headers = values.Select(h => h.Trim()).ToArray();
                continue;
            }

            if (headers == null || values.Length != headers.Length)
                throw new FormatException($"Línea {lineNumber} mal formada.");

            var instance = new T();

            for (int i = 0; i < headers.Length; i++)
            {
                var header = headers[i];
                if (!properties.TryGetValue(header, out var prop)) continue;

                try
                {
                    var convertedValue = Convert.ChangeType(values[i].Trim(), prop.PropertyType);
                    prop.SetValue(instance, convertedValue);
                }
                catch
                {
                    throw new FormatException($"Error en línea {lineNumber}, columna '{header}'");
                }
            }

            result.Add(instance);
        }
        return result;
    }
}
