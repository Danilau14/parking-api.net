namespace ParkingApi.Application.Common.Converters;

public class EnumConverter<TEnum> : IPropertyConverter where TEnum : struct, Enum
{
    public object FromEntry(DynamoDBEntry entry)
    {
        if (entry == null || string.IsNullOrEmpty(entry.AsString()))
        {
            return default(TEnum);
        }

        if (Enum.TryParse(typeof(TEnum), entry.AsString(), out var value))
        {
            return value;
        }

        throw new ArgumentException($"El valor {entry} no es válido para el enum {typeof(TEnum).Name}.");
    }

    public DynamoDBEntry ToEntry(object value)
    {
        if (value == null)
        {
            return null;
        }

        if (Enum.IsDefined(typeof(TEnum), value))
        {
            return new Primitive(value.ToString());
        }

        throw new ArgumentException($"El valor {value} no es válido para el enum {typeof(TEnum).Name}.");
    }
}