using Amazon.CDK.AWS.IAM;

namespace ArturRios.Common.Aws.Configuration;

public class CfnPolicyDocument
{
    public string Version { get; set; } = "2008-10-17";
    public List<StatementEntry> Statements { get; set; } = [];

    // ReSharper disable once ClassNeverInstantiated.Global
    // Reason: needed for it's side effects
    public class StatementEntry
    {
        public string Effect { get; set; } = string.Empty;
        public PrincipalBase Principal { get; set; } = null!;
        public string Action { get; set; } = string.Empty;
        public string Resource { get; set; } = string.Empty;
    }
}
