namespace ArturRios.Common.Aws.Command;

public class AwsCommandQueueConfiguration
{
    public required string QueueUrl { get; set; }
    public required string QueueArn { get; set; }
    public required string SchedulerRoleArn { get; set; }
}
