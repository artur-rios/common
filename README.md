# Common

## Important Notice

I do not update this repository anymore. I changed the approach by building separate libraries for each utility, which allows for better versioning and dependency management.
You can find the new libraries here:

- [dotnet-configuration](https://github.com/artur-rios/dotnet-configuration)
- [dotnet-data](https://github.com/artur-rios/dotnet-data)
- [dotnet-extensions](https://github.com/artur-rios/dotnet-extensions)
- [dotnet-logging](https://github.com/artur-rios/dotnet-logging)
- [dotnet-output](https://github.com/artur-rios/dotnet-output)
- [dotnet-util](https://github.com/artur-rios/dotnet-util)
- [dotnet-web-api-util](https://github.com/artur-rios/dotnet-web-api-util)

Not everything has been migrated yet, but I plan to do so in the future. If you have any questions or need assistance, feel free to reach out.
You can still use this repository, but I recommend checking out the new libraries for a more up-to-date experience. However, the new libraries are targeting .NET 10, cause I intend to always use the latest versions of .NET for test purposes. This collection of libraries will still be targeting .NET 8.

## Overview

A collection of reusable utilities for .NET projects, designed to be shared across multiple solutions. This library is
structured into modular subprojects, so you can reference only what you need.

I maintain this in my free time, and it's a space where I explore ideas, experiment with patterns, and occasionally
indulge in some overengineering. Contributions are very welcome—feel free to jump in!

## Can I use it in real projects?

Yes, you can. Most features are actively used in my own projects and are tested accordingly. The goal is to achieve full
unit test coverage across the library — while we're not there yet, it's a work in progress.

That said, some components are experimental and may evolve over time. I recommend reviewing the code before integrating
it into production. If you spot something odd or want to improve it, feel free to open an issue, submit a pull request,
or fork the repo and make it your own.

Liked it? Please consider giving a star to the repository.

## Recommended Use

This library is designed to be added as a Git submodule to your .NET solution, making it easy to share and reuse code
across multiple projects.

### Add as a Submodule

To include this library in your repository:

```bash
git submodule add https://github.com/artur-rios/common.git common
git submodule update --init --recursive
```

### Reference Only What You Need

The library is split into multiple projects, each targeting a specific set of utilities or patterns. This modular
structure allows you to reference only the parts you need:

1. Open your solution in your favorite IDE (e.g., Visual Studio).
2. Add the projects you need as references to your existing projects, by browsing to the submodule folder (e.g.,
   libs/common) and select the specific .csproj files you want to include
3. Build and you're good to go!

## Projects Structure

### Attributes
**Folder:** `src/ArturRios.Common.Attributes/`
- **Root classes:** `InjectionValidatorAttribute`
- **Subdirectories:**
  - `EndpointToggle/`: `EndpointToggleAttribute`, `EndpointDisabledException`

### Aws
**Folder:** `src/ArturRios.Common.Aws/`
- **Subdirectories:**
  - `CloudFormation/`: `CloudFormationBuilder`, `CloudFormationResourcesFactory`, `CloudFormationSetup`, `CloudFormationStack`
  - `Command/`: `AwsCommandQueue`, `AwsCommandQueueConfiguration`
  - `Configuration/`: `CfnPolicyDocument`
  - `Extensions/`: `TagManagerExtensions`
  - `Lambda/`: `LambdaEntryPoint`, `LambdaFunction`
    - `Config/`: `LogType`
    - `Logging/`: `LambdaLogTracer`, `LambdaLogTracerSingleton`
      - `Interfaces/`: `ILambdaLogTracer`, `ILambdaLogTracerSingleton`
  - `Middleware/`: `GuidTraceIdentifierMiddleware`
  - `RestApi/`: `AwsRestApi`, `AwsRestApiDeployment`, `AwsRestApiKey`, `AwsRestApiResource`, `AwsRestApiResourceMethod`, `AwsRestApiStage`, `AwsRestApiUsagePlan`, `AwsRestApiUsagePlanKey`
  - `Sns/`: `ISnsService`, `SnsSubscription`, `SnsTopic`
  - `Sqs/`: `ISqsMessageHandler`, `SqsEntryPoint`, `SqsQueue`

### Configuration
**Folder:** `src/ArturRios.Common.Configuration/`
- **Subdirectories:**
  - `Enums/`: `ConfigurationSourceType`, `DataFormatType`, `DataSource`, `EnvironmentType`, `OutputType`
  - `Loaders/`: `ConfigurationLoader`
  - `Providers/`: `EnvironmentProvider`, `SettingsProvider`
    - `Interfaces/`: `IConfigurationProvider`

### Custom
**Folder:** `src/ArturRios.Common.Custom/`
- **Root classes:** `CustomConsole`

### Data
**Folder:** `src/ArturRios.Common.Data/`
- **Root classes:** `BaseDataClient`, `Entity`
- **Subdirectories:**
  - `Configuration/`: `BaseDbContextOptions`
  - `Interfaces/`: `ICrudRepository`, `IRangeRepository`, `IReadOnlyRepository`
  - `Util/`: `ValueConverterCollection`

### Extensions
**Folder:** `src/ArturRios.Common.Extensions/`
- **Root classes:** `ComparisonExtensions`, `DateTimeExtensions`, `EnumerableExtensions`, `EnumExtensions`, `ExceptionExtensions`, `FluentValidationExtensions`, `GenericExtensions`, `ObjectExtensions`, `QueryableExtensions`, `StringExtensions`

### Logging
**Folder:** `src/ArturRios.Common.Logging/`
- **Root classes:** `ConsoleAnsi`, `CustomLogLevel`, `StandaloneLogger`, `StateLogger`
- **Subdirectories:**
  - `Adapter/`: `LoggerCallerExtensions`, `MicrosoftLoggerAdapter`, `MicrosoftLoggerBuilderExtensions`, `MicrosoftLoggerProvider`
  - `Configuration/`: `ConsoleAnsiColors`, `ConsoleLoggerConfiguration`, `FileLoggerConfiguration`, `LogFolderScheme`, `LoggerConfiguration`, `LoggerType`, `LogSplitLevel`
  - `Factories/`: `InternalLoggerFactory`, `LogEntryFactory`
  - `Interfaces/`: `IInternalLogger`, `IStandaloneLogger`, `IStateLogger`
  - `Loggers/`: `ConsoleLogger`, `FileLogger`

### Messaging
**Folder:** `src/ArturRios.Common.Messaging/`
- **Root classes:** `IEmailService`, `MailgunEmailService`

### Output
**Folder:** `src/ArturRios.Common.Output/`
- **Root classes:** `CustomException`, `DataOutput`, `PaginatedOutput`, `ProcessOutput`

### Pipelines
**Folder:** `src/ArturRios.Common.Pipelines/`
- **Root classes:** `Pipeline`
- **Subdirectories:**
  - `Commands/`: `Command`, `CommandOutput`
    - `Interfaces/`: `ICommandHandler`, `ICommandHandlerAsync`, `ICommandQueue`
    - `Queues/`: `CustomCommandQueue`, `SerializedCommand`, `SerializedCommandOutput`
  - `Events/`: `DomainEvent`, `DomainEventBus`, `DomainEventEntity`, `DomainEventHandlerWrapper`, `EventHandlerWrapper`
    - `Interfaces/`: `IDomainEventBus`, `IDomainEventHandler`, `IEventHandler`
  - `Messaging/`: `IMessageHandler`, `INotificationService`, `Notification`
  - `Queries/`: `IQueryHandler`, `IQueryHandlerAsync`, `ISingleQueryHandler`, `ISingleQueryHandlerAsync`, `Query`, `QueryOutput`

### ProtoBuf
**Folder:** `src/ArturRios.Common.ProtoBuf/`
- **Root classes:** `Extensions`, `ProtoBufService`

### Security
**Folder:** `src/ArturRios.Common.Security/`
- **Root classes:** `JwtToken`, `JwtTokenConfiguration`, `JwtTokenConfigurationValidator`

### Test
**Folder:** `src/ArturRios.Common.Test/`
- **Root classes:** `CustomAssert`, `TestException`, `WebApiTest`
- **Subdirectories:**
  - `Attributes/`: `CustomAttributes`, `CustomFactAttribute`, `CustomTheoryAttribute`
  - `Mock/`: `FakeRepository`, `FakeScheduler`

### Util
**Folder:** `src/ArturRios.Common.Util/`
- **Root classes:** `Retry`
- **Subdirectories:**
  - `Collections/`: `CharacterCollection`
  - `Condition/`: `Condition`, `ConditionFailedException`
  - `Files/`: `FileReader`, `FileReaderAsync`
  - `Hashing/`: `Hash`, `HashConfiguration`
  - `Random/`: `CustomRandom`, `RandomStringOptions`
  - `RegularExpressions/`: `RegexBuilder`, `RegexCollection`, `RegexExtensions`
  - `Waiter/`: `JitteredWaiter`, `MaxRetriesReachedException`

### Validation
**Folder:** `src/ArturRios.Common.Validation/`
- **Root classes:** `FluentValidator`, `IFluentValidator`

### Web
**Folder:** `src/ArturRios.Common.Web/`
- **Subdirectories:**
  - `Api/`:
    - `Client/`: `BaseWebApiClient`, `BaseWebApiClientRoute`
    - `Configuration/`: `AppSettingsKeys`, `WebApiParameters`, `WebApiStartup`
  - `AspNetCore/`: `ResponseResolver`
  - `Handlers/`: `TracePropagationHandler`
  - `Http/`: `HttpGateway`, `HttpOutput`, `HttpStatusCodes`
  - `Middleware/`: `ExceptionMiddleware`, `TraceActivityMiddlware`, `WebApiMiddleware`
  - `Security/`:
    - `Attributes/`: `AllowAnonymousAttribute`, `AuthorizeAttribute`, `RoleRequirementAttribute`
    - `Extensions/`: `AuthenticationExtensions`
    - `Filters/`: `RoleRequirementFilter`
    - `Interfaces/`: `IAuthenticationProvider`
    - `Middleware/`: `JwtMiddleware`
    - `Providers/`: `TokenProvider`
    - `Records/`: `AuthenticatedUser`, `Authentication`, `Credentials`
    - `Validation/`: `CredentialsValidator`

## Features

- [Data access utilities](docs/data.md)
- [Web utilities](docs/web.md)

*Documentation under construction*
