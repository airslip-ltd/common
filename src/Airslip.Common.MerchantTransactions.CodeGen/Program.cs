using Airslip.Common.Testing;
using NSwag;
using NSwag.CodeGeneration.CSharp;
using NSwag.CodeGeneration.OperationNameGenerators;

OpenApiDocument document = await OpenApiDocument.FromUrlAsync("http://localhost:7071/swagger.json");
string fileName = "GeneratedRetailerApiV1Client";
            
CSharpClientGeneratorSettings clientSettings = new()
{
    ClassName = fileName,
    CSharpGeneratorSettings =
    {
        Namespace = "Airslip.Common.MerchantTransactions"
    },
    GenerateClientInterfaces = true,
    OperationNameGenerator = new SingleClientFromOperationIdOperationNameGenerator(),
    ClientBaseClass = "MerchantIntegrationApi",
    ClientBaseInterface = "IMerchantIntegrationApi",
    UseHttpRequestMessageCreationMethod = true
};

CSharpClientGenerator clientGenerator = new(document, clientSettings);
string? code = clientGenerator.GenerateFile();
            
string commonLibrary = "Airslip.Common.MerchantTransactions";
string workingDirectory = Path.Combine(OptionsMock.GetBasePath(commonLibrary)!);
Directory.CreateDirectory(workingDirectory);

string path = Path.Combine(workingDirectory, $"{fileName}.cs");

if (!File.Exists(path))
{
    await File.Create(path).DisposeAsync();
}

await using StreamWriter tw = new(path);
await tw.WriteLineAsync(code);
