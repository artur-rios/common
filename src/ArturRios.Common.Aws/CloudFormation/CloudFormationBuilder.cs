using System.Text.Json;
using Amazon.CDK;
using Amazon.CDK.CXAPI;
using ArturRios.Common.Custom;

namespace ArturRios.Common.Aws.CloudFormation;

public sealed class CloudFormationBuilder
{
    private static readonly JsonSerializerOptions s_jsonSerializerOptions = new() { WriteIndented = true };

    public static void Main(CloudFormationSetup setup, string outputPath)
    {
        var app = CreateDefaultApp();
        setup.Init(app);

        var assembly = app.Synth();

        PrintStacksToConsole(assembly);
        SaveTemplateToFile(assembly, outputPath);
    }

    private static App CreateDefaultApp()
    {
        return new App(new AppProps
        {
            DefaultStackSynthesizer = new DefaultStackSynthesizer(new DefaultStackSynthesizerProps
            {
                GenerateBootstrapVersionRule = false, Qualifier = null
            })
        });
    }

    private static void PrintStacksToConsole(CloudAssembly assembly)
    {


        foreach (var artifact in assembly.Stacks)
        {
            CustomConsole.WriteCharLine();
            Console.WriteLine($"Stack name: {artifact.StackName}");
            Console.WriteLine();
            Console.Write(JsonSerializer.Serialize(artifact.Template, s_jsonSerializerOptions));
            Console.WriteLine();
            CustomConsole.WriteCharLine();
        }
    }

    private static void SaveTemplateToFile(CloudAssembly assembly, string path)
    {
        foreach (var artifact in assembly.Stacks)
        {
            var outputPath = Path.Combine(path, "output", $"{artifact.StackName.ToLower()}-template.json");
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

            var jsonContent = JsonSerializer.Serialize(artifact.Template, s_jsonSerializerOptions);

            File.WriteAllText(outputPath, jsonContent);
            Console.WriteLine($"Template for stack '{artifact.StackName}' saved to: {outputPath}");
        }
    }
}
