namespace C2Server.Models.HTTP;

public class Cookie
{
    public string Name { get; set; }
    public string Value { get; set; }
    public DateTime? Expiration { get; set; } // Nullable for session cookies
    public string Domain { get; set; }
    public string Path { get; set; }
    public bool Secure { get; set; }
    public bool HttpOnly { get; set; }

    public Cookie(string name, string value)
    {
        Name = name;
        Value = value;
        Domain = string.Empty;
        Path = "/";
        Secure = false;
        HttpOnly = false;
    }

    public string ToSetCookieHeader()
    {
        var cookieString = $"{Name}={Value}; Path={Path};";

        if (Expiration.HasValue)
        {
            cookieString += $" Expires={Expiration.Value.ToUniversalTime():R};";
        }

        if (!string.IsNullOrEmpty(Domain))
        {
            cookieString += $" Domain={Domain};";
        }

        if (Secure)
        {
            cookieString += " Secure;";
        }

        if (HttpOnly)
        {
            cookieString += " HttpOnly;";
        }

        return cookieString.TrimEnd(';');
    }
}
