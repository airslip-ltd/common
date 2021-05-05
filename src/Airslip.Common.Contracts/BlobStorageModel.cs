using System.IO;

namespace Airslip.Common.Contracts
{
    public record BlobStorageModel(
        Stream Data, 
        string Name, 
        string ContentType);
}