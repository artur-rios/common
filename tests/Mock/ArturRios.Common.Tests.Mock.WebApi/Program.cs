namespace ArturRios.Common.Tests.Mock.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var startup = new Startup(args);

        startup.BuildAndRun();
    }
}
