namespace ArturRios.Common.Custom;

public static class CustomConsole
{
    public static void WriteCharLine(char c = '-', int quantity = 100)
    {
        System.Console.WriteLine(new string(c, quantity));
    }
}
