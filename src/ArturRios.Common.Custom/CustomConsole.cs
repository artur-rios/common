namespace ArturRios.Common.Custom;

public static class CustomConsole
{
    public static void WriteCharLine(char c = '-', int quantity = 100) => Console.WriteLine(new string(c, quantity));
}
