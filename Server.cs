using System.Net;
using System.Net.Sockets;
using C2Server.Handler;

namespace C2Server;

public class Server
{
    private IPAddress listenerIP = IPAddress.Loopback;
    private int listenerPort = Config.Port;
    private IHandler handler = HandlerFactory.CreateHandler();

    public void Start(string version)
    {
        try
        {
            var listener = new TcpListener(listenerIP, listenerPort);
            listener.Start();

            PrintServerInfo(version, listenerIP, listenerPort);

            Task.Run(async () =>
            {
                while (true)
                {
                    var client = await listener.AcceptTcpClientAsync();
                    _ = RequestIO(client);
                }
            });
        }
        catch (Exception e)
        {
            Log.Error($"Unable to start server: {e.Message}");
        }
    }

    private async Task RequestIO(TcpClient client)
    {
        using (client)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            byte[] responseBytes = await handler.HandleRequest(buffer, bytesRead);

            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
    }

    public void SetupRoute(string method, string path, RequestHandler requestHandler)
    {
        handler.RegisterRoute(method, path, requestHandler);
    }

    private void PrintServerInfo(string version, IPAddress host, int port)
    {
        int width = 50;
        int margin = 10;
        string color = "\u001b[94m";
        string reset = "\u001b[90m";

        Console.WriteLine(reset);
        Console.WriteLine(new string(' ', margin) + "┌" + new string('─', width - 2) + "┐");
        Console.WriteLine(new string(' ', margin) + $"│{new string(' ', width - 2)}│");
        Console.WriteLine(new string(' ', margin) + $"|{color}{PadCenter($" C2Server {version}", width)}{reset}|");
        Console.WriteLine(new string(' ', margin) + $"│{new string(' ', width - 2)}│");
        Console.WriteLine(new string(' ', margin) + $"│{PadCenter($"Host ..... {color}{host}{reset}      Port ..... {color}{port}{reset}", width + 20)}│");
        Console.WriteLine(new string(' ', margin) + $"│{new string(' ', width - 2)}│");
        Console.WriteLine(new string(' ', margin) + "└" + new string('─', width - 2) + "┘");
        Console.WriteLine();
    }

    static string PadCenter(string text, int totalWidth)
    {
        int spacesNeeded = totalWidth - 2 - text.Length;
        int leftPadding = spacesNeeded / 2;
        int rightPadding = spacesNeeded - leftPadding;
        return new string(' ', leftPadding) + text + new string(' ', rightPadding);
    }
}