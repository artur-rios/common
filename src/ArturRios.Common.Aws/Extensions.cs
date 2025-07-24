using Amazon.CDK;

namespace ArturRios.Common.Aws;

public static class Extensions
{
    public static void SetDefaultTags(this TagManager tags)
    {
        var env = Fn.FindInMap(Fn.Ref("Stage"), Fn.Ref("AWS::Region"), "Environment");

        tags.SetTag("business_criticality", "Medium");
        tags.SetTag("env", env);
        tags.SetTag("product_name", "test");
        tags.SetTag("product_team", "test-team");
        tags.SetTag("risk_exposure", "Medium");
        tags.SetTag("system", "test-system");
    }
}
