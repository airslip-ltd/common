using FluentAssertions;
using Xunit;

namespace Airslip.Common.Types.Tests
{
    public class StorageFunctionTests
    {
        [Fact]
        public void Can_build_standard_blob_name_with_multiple_parameter()
        {
           string blobName = StorageFunctions.BuildBlobName("MerchantIntegrations", "PosProviders","12345");

           string expectedBlobName = "merchant-integrations/pos-providers/12345";
           
           blobName.Should().Be(expectedBlobName);
        }
        
        [Fact]
        public void Can_build_standard_blob_name_with_one_parameter()
        {
            string blobName = StorageFunctions.BuildBlobName("Merchant");

            string expectedBlobName = "merchant";
           
            blobName.Should().Be(expectedBlobName);
        }
        
        [Fact]
        public void Empty_blob_does_not_error()
        {
            string blobName = StorageFunctions.BuildBlobName();

            string expectedBlobName = "";
           
            blobName.Should().Be(expectedBlobName);
        }
    }
}