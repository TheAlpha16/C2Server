namespace C2Server;

public class Config
{
    static Config()
    {
        DotNetEnv.Env.Load();
    }

    public static int Port => int.TryParse(Environment.GetEnvironmentVariable("PORT"), out int port) ? port : 8989;
    public static string protocol => Environment.GetEnvironmentVariable("PROTOCOL") ?? "HTTP";
}