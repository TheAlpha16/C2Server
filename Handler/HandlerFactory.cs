namespace C2Server.Handler;

public interface IHandler
{
    Task<byte[]> HandleRequest(byte[] requestBytes, int requestLength);
    public void RegisterRoute(string method, string path, RequestHandler handler);
}

public static class HandlerFactory
{
    public static IHandler CreateHandler()
    {
        string protocol = Config.protocol;

        return protocol switch
        {
            "HTTP" => new HTTPHandler(),

            _ => throw new NotSupportedException($"Protocol '{protocol}' is not supported.")
        };
    }
}