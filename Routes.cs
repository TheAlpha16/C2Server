using C2Server.Models.HTTP;

namespace C2Server;

class Routes
{
    public async static Task<HTTPResponse> Home(HTTPRequest request)
    {
        return new HTTPResponse().Text("Welcome to C2Server!");
    }

    public async static Task<HTTPResponse> Ping(HTTPRequest request)
    {
        return new HTTPResponse().Text("Pong!");
    }
}