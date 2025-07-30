using ArturRios.Common.Aws.Sqs;

namespace ArturRios.Common.Aws.Tests.Lambda;

public class LambdaTestEntryPoint : SqsEntryPoint<SqsMessageHandler>;
