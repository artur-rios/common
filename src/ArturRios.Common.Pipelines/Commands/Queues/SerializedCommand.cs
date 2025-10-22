using System.Text.Json;

namespace ArturRios.Common.Pipelines.Commands.Queues;

public class SerializedCommand : Command
{
    public SerializedCommand(string typeFullName, string assemblyName, object data, Guid commandId)
    {
        TypeFullName = typeFullName;
        AssemblyName = assemblyName;
        CommandId = commandId;

        if (data is JsonElement { ValueKind: JsonValueKind.Object } json)
        {
            var assemblyChain = AppDomain.CurrentDomain.GetAssemblies()
                .First(assembly => assembly.GetName().Name == assemblyName);
            var commandType = assemblyChain.GetType(typeFullName);

            if (commandType is null)
            {
                throw new ArgumentException($"Type {typeFullName} does not exist in assembly {assemblyName}");
            }

            var deserializedData = json.Deserialize(commandType);

            Data = deserializedData ??
                   throw new ArgumentException($"Failed to deserialize data for type {commandType}");
        }
        else
        {
            Data = data;
        }
    }

    private SerializedCommand(object commandInstance)
    {
        TypeFullName = commandInstance.GetType().FullName ?? throw new ArgumentNullException(nameof(commandInstance),
            "Command instance type full name cannot be null");
        AssemblyName = commandInstance.GetType().Assembly.GetName().Name!;
        Data = commandInstance;
        CommandId = Guid.NewGuid();
    }

    public string TypeFullName { get; }
    public string AssemblyName { get; }
    public object Data { get; }
    public Guid CommandId { get; }
    public bool IsScheduled { get; private set; }
    public DateTime? ScheduledAt { get; private set; }

    public static SerializedCommand FromRequest<TRequest>(TRequest request) where TRequest : notnull => new(request);

    public static SerializedCommand FromScheduledRequest<TRequest>(TRequest request, DateTime scheduledAt)
        where TRequest : notnull
    {
        var command = new SerializedCommand(request) { IsScheduled = true, ScheduledAt = scheduledAt };

        return command;
    }

    public static SerializedCommand FromJson(string json) =>
        JsonSerializer.Deserialize<SerializedCommand>(json) ??
        throw new ArgumentException("Failed to deserialize JSON to SerializedCommand");

    public string ToJson() => JsonSerializer.Serialize(this);
}
