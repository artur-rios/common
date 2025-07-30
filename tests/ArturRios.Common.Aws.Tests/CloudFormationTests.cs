using System.Runtime.CompilerServices;
using ArturRios.Common.Aws.CloudFormation;
using ArturRios.Common.Aws.Tests.Setup;

namespace ArturRios.Common.Aws.Tests;

public class CloudFormationTests
{
    [Fact]
    public void Should_CreateServerlessTemplate()
    {
        var resourcesFactory = new ResourcesFactory();
        var setup = new TestCloudFormationSetup();
        var outputPath = GetCurrentPath()!;
        var outputDir = Path.Combine(outputPath, "output");

        if (Directory.Exists(outputDir))
        {
            Directory.Delete(outputDir, true);
        }

        CloudFormationBuilder.Main(resourcesFactory, setup, outputPath);

        var expectedFile = Path.Combine(outputDir, "cf-test-stack-template.json");
        Assert.True(File.Exists(expectedFile), $"Expected file not found: {expectedFile}");
    }

    private static string? GetCurrentPath([CallerFilePath] string? path = null)
    {
        return path == null ? null : Path.GetDirectoryName(path);
    }
}
