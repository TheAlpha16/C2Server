namespace C2Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string version = "v0.1.0";

            Server server = new Server();
            SetupRoutes(server);
            server.Start(version);

            await Task.Delay(-1);
        }

        static void SetupRoutes(Server server)
        {
            server.SetupRoute(
                "GET", "/", Routes.Home
            );

            server.SetupRoute(
                "GET", "/ping", Routes.Ping
            );
        }
    }
}