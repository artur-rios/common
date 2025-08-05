using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SQS;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;

namespace ArturRios.Common.Aws.Sqs;

public class SqsEntryPoint<THandler> where THandler : class, ISqsMessageHandler
{
    private struct EntryPointNames
    {
        public Type EntryPointLoggerType;
        public string EntryPointName;
    }

    private object collectionLock = new();
    private IServiceCollection? _serviceCollection = null;
    private IServiceProvider? _serviceProvider = null;
    private EntryPointNames Names;

    protected IServiceProvider ServiceProvider
    {
        get
        {
            lock (collectionLock)
            {
                // ReSharper disable once InvertIf
                if (_serviceCollection is null)
                {
                    _serviceCollection = new ServiceCollection();
                    ConfigureServices(_serviceCollection);

                    Names = new EntryPointNames
                    {
                        EntryPointLoggerType = typeof(ILogger<>).MakeGenericType(GetType()),
                        EntryPointName = GetType().Name
                    };
                }

                return _serviceProvider ??= _serviceCollection.BuildServiceProvider(new ServiceProviderOptions
                {
                    ValidateOnBuild = true, ValidateScopes = true
                });
            }
        }
    }

    private async Task<SQSBatchResponse.BatchItemFailure?> Handle(SQSEvent.SQSMessage message)
    {
        try
        {
            LambdaLogger.Log($"SQS Input: {message.Body}");

            using var scope = ServiceProvider.CreateScope();

            var handler = scope.ServiceProvider.GetRequiredService<THandler>();
            var result = await handler.HandleAsync(message);
            var approximateReceiveCountAttributeValue =
                message.Attributes.GetValueOrDefault(MessageSystemAttributeName.ApproximateReceiveCount,
                    defaultValue: null);
            var approximateReceiveCount = 0;

            if (approximateReceiveCountAttributeValue is not null)
            {
                approximateReceiveCount = int.Parse(approximateReceiveCountAttributeValue);
            }

            if (!result.Success && approximateReceiveCount < 3)
            {
                return new SQSBatchResponse.BatchItemFailure { ItemIdentifier = message.MessageId };
            }

            return null;
        }
        catch (Exception ex)
        {
            var logger = (ILogger)_serviceProvider!.GetRequiredService(typeof(ILogger<>).MakeGenericType(GetType()));

            logger.LogError(ex, "Message handling failed");

            return new SQSBatchResponse.BatchItemFailure { ItemIdentifier = message.MessageId };
        }
    }

    public async Task<SQSBatchResponse> Main(SQSEvent sqsEvent)
    {
        var entryPointLogger = (ILogger)ServiceProvider.GetRequiredService(Names.EntryPointLoggerType);

        entryPointLogger.LogDebug(
            "Begin entry point: {EntryPointName} - Batch size: {BatchSize} - Identifiers: {Identifiers}",
            Names.EntryPointName, sqsEvent.Records.Count, sqsEvent.Records.Select(record => record.MessageId));

        var tasks = new List<Task<SQSBatchResponse.BatchItemFailure?>>();

        foreach (var record in sqsEvent.Records)
        {
            tasks.Add(Handle(record));
        }

        var results = await Task.WhenAll(tasks);

        entryPointLogger.LogDebug("End entry point: {EntryPointName}", Names.EntryPointName);

        return new SQSBatchResponse { BatchItemFailures = results.Where(result => result is not null).ToList() };
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        services.TryAddScoped<THandler>();

        // Add any other dependencies, like services, database, logging, etc
    }
}
