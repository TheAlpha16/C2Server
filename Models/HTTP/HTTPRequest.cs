using System.Text.RegularExpressions;

namespace C2Server.Models.HTTP;

public class HTTPRequest
{
    public string Version { get; set; }
    public string Method { get; set; }
    public string Path { get; set; }
    public Dictionary<string, string> Headers { get; set; }
    public Dictionary<string, string> Queries { get; set; }
    public Dictionary<string, string> Cookies { get; set; }
    public string Body { get; set; }

    public HTTPRequest(string rawRequest)
    {
        var lines = rawRequest.Split("\r\n");
        Headers = new Dictionary<string, string>();
        Queries = new Dictionary<string, string>();
        Cookies = new Dictionary<string, string>();

        if (lines.Length < 1)
        {
            throw new BadRequestException("Empty Request");
        }

        // starting line
        var requestLine = lines[0].Trim().Split(' ');

        if (requestLine.Length != 3)
        {
            throw new BadRequestException();
        }
        else
        {
            Method = requestLine[0];
            Version = requestLine[2];

            int queryIndex = requestLine[1].IndexOf("?");

            if (queryIndex == -1)
            {
                Path = requestLine[1];
            }
            else
            {
                Path = requestLine[1].Substring(0, queryIndex);
                var queries = requestLine[1].Substring(queryIndex + 1).Trim().Split('&');

                foreach (var query in queries)
                {
                    var keyValue = query.Split('=');
                    if (keyValue.Length != 2)
                    {
                        throw new BadRequestException("Invalid query parameters");
                    }
                    Queries[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }
        }

        // headers
        int i = 1;
        while (i < lines.Length && !string.IsNullOrWhiteSpace(lines[i]))
        {
            var header = lines[i].Split(':', 2);
            if (header.Length == 2)
            {
                Headers[header[0].Trim()] = header[1].Trim();
            }
            i++;
        }

        // cookies
        if (Headers.ContainsKey("Cookie"))
        {
            var cookieKVPairs = Headers["Cookie"].Split(';');

            foreach (var cookieString in cookieKVPairs)
            {
                var cookie = cookieString.Trim().Split('=');
                if (cookie.Length != 2)
                {
                    throw new BadRequestException("Invalid Cookies");
                }
                Cookies[cookie[0].Trim()] = cookie[1].Trim();
            }
        }

        // body
        Body = i + 1 < lines.Length ? string.Join("\r\n", lines.Skip(i + 1)) : string.Empty;
        Validate();
    }

    private void Validate()
    {
        // version
        if (!Regex.IsMatch(Version, @"^HTTP/(\d+).(\d+)$"))
        {
            throw new BadRequestException("Invalid HTTP version");
        }

        // method 
        Method = Method.ToUpper();
    }
}
