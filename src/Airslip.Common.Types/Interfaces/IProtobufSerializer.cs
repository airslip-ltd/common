namespace Airslip.Common.Types.Interfaces
{
    public interface IProtobufSerializer<T>
    {
        byte[] Serialize(T value);
        T Deserialize(byte[] value);
    }
}