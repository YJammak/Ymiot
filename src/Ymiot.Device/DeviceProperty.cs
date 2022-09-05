namespace Ymiot.Device;

public class DeviceProperty
{
    public string Name { get; }

    public int SymbolId { get; }

    public int PropertyId { get; }

    public ValueType ValueType { get; }

    public string ValueUnit { get; }

    public IReadOnlyDictionary<object, string> ValueDescription { get; }

    public Permission Permission { get; }

    public DeviceProperty(
        string name,
        int symbolId,
        int propertyId,
        ValueType valueType,
        string valueUnit,
        IReadOnlyDictionary<object, string> valueDescription,
        Permission permission)
    {
        Name = name;
        SymbolId = symbolId;
        PropertyId = propertyId;
        ValueType = valueType;
        ValueUnit = valueUnit;
        Permission = permission;
        ValueDescription = valueDescription;
    }
}
