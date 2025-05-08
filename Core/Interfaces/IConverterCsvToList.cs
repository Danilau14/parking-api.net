namespace ParkingApi.Core.Interfaces;

public interface IConverterCsvToList
{
    Task<List<T>> Converter<T>(IFormFile file) where T : new();
}
