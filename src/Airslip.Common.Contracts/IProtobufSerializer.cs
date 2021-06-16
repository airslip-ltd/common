namespace Airslip.Common.Contracts
{
    public interface IProtobufSerializer<T>
    {
        byte[] Serialize(T value);
        T Deserialize(byte[] value);
    }
}