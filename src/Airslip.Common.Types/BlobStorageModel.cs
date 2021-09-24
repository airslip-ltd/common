using System.IO;

namespace Airslip.Common.Types
{
    public record BlobStorageModel(
        Stream Data, 
        string Name, 
        string ContentType);
}