using Amazon.CDK.AWS.IAM;

namespace ArturRios.Common.Aws;

public class CfnPolicyDocument
{
    public string Version { get; set; } = "2008-10-17";
    public List<StatementEntry> Statements { get; set; } = [];

    public class StatementEntry
    {
        public string Effect { get; set; }
        public PrincipalBase Principal { get; set; }
        public string Action { get; set; }
        public string Resource { get; set; }
    }
}
