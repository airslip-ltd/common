using System.IO;

namespace Airslip.Common.ImageGeneration
{
    public interface IQrCodeService
    {
        MemoryStream GenerateQrCodeImageForAnyString(string input, int pixelsPerModule = 20);
        MemoryStream GenerateQrCodeImageForUrl(string url, int pixelsPerModule = 20);
    }
}