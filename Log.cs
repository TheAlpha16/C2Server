namespace C2Server;

public class Log
{
    private static string Now() {
        return DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
    }

    public static void WriteString(string prefix, string message, ConsoleColor color) {
        Console.ForegroundColor = color;
        Console.WriteLine($"[{prefix}] {Now()} {message}");
        Console.ResetColor();
    }

    public static void Info(string message) => WriteString("*", message, ConsoleColor.Blue);

    public static void Error(string message) => WriteString("!", message, ConsoleColor.Red);

    public static void Success(string message) => WriteString("+", message, ConsoleColor.Green);

    public static void Debug(string message) => WriteString("#", message, ConsoleColor.Yellow);

    public static void Access(string message) => WriteString("-", message, ConsoleColor.White);

}