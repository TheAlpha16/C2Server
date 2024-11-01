using System.Text;
using System.Text.Json;

namespace C2Server.Models.HTTP;

public class HTTPResponse
{
    public string Version { get; set; } = "HTTP/1.1";
    public HTTPStatus Status { get; set; } = HTTPStatus.OK;
    public Dictionary<string, string> Headers { get; set; } = new();
    public List<Cookie> Cookies { get; set; } = new();
    public byte[] Body { get; set; } = Array.Empty<byte>();

    public HTTPResponse() { }

    // Method to set JSON response
    public HTTPResponse JSON(object data)
    {
        Body = JsonSerializer.SerializeToUtf8Bytes(data);
        Headers["Content-Type"] = "application/json";

        return this;
    }

    // Method to set byte array response
    public HTTPResponse Bytes(byte[] data)
    {
        Body = data;
        Headers["Content-Type"] = "application/octet-stream";

        return this;
    }

    // Method to set a plain text response
    public HTTPResponse Text(string text)
    {
        Body = Encoding.UTF8.GetBytes(text);
        return this;
    }

    public HTTPResponse SetStatus(HTTPStatus status)
    {
        Status = status;
        return this;
    }

    public void SetCookie(Cookie cookie)
    {
        Cookies.Add(cookie);
    }

    public byte[] Send()
    {
        if (Body.Length != 0 && !Headers.ContainsKey("Content-Type"))
            Headers["Content-Type"] = "text/plain";

        Headers["Content-Length"] = Body.Length.ToString();

        var responseBuilder = new StringBuilder();
        responseBuilder.AppendLine($"{Version} {(int)Status} {Status.Message()}");

        foreach (var header in Headers)
        {
            responseBuilder.AppendLine($"{header.Key}: {header.Value}");
        }

        foreach (var cookie in Cookies)
        {
            responseBuilder.AppendLine($"Set-Cookie: {cookie.ToSetCookieHeader()}");
        }

        responseBuilder.AppendLine();
        byte[] headerBytes = Encoding.UTF8.GetBytes(responseBuilder.ToString());

        return headerBytes.Concat(Body).ToArray();
    }
}

