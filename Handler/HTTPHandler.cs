using System.Text;
using C2Server.Models.HTTP;

namespace C2Server.Handler;

public delegate Task<HTTPResponse> RequestHandler(HTTPRequest request);

public class HTTPHandler : IHandler
{
    private readonly Dictionary<(string Method, string Path), RequestHandler> _routes = new();

    public async Task<byte[]> HandleRequest(byte[] requestBytes, int requestLength)
    {
        string requestString = string.Empty;

        try
        {
            requestString = Encoding.UTF8.GetString(requestBytes, 0, requestLength);
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
        }

        try
        {
            HTTPRequest request = new HTTPRequest(rawRequest: requestString);

            if (_routes.TryGetValue((request.Method.ToUpper(), request.Path), out var handler))
            {
                HTTPResponse response = await handler(request);

                Log.Access($"{request.Method} {request.Path} | {(int)response.Status}");
                return response.Send();
            }
            else
            {
                Log.Access($"{request.Method} {request.Path} | {(int)HTTPStatus.NotFound}");
                return new HTTPResponse().SetStatus(HTTPStatus.NotFound).Send();
            }
        }
        catch (HTTPException ex)
        {
            return new HTTPResponse().SetStatus(ex.StatusCode).Send();
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return new HTTPResponse().SetStatus(HTTPStatus.InternalServerError).Send();
        }
    }

    public void RegisterRoute(string method, string path, RequestHandler handler)
    {
        _routes[(method.ToUpper(), path)] = handler;
    }
}