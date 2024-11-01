# C2 Server

An efficient and lightweight C2 (Command and Control) server built for secure communication and control over remote devices or clients in real-time. This server offers flexible routing, request handling, and built-in cookie management, designed for custom integration with tools and security projects.

## Features

- **Flexible Request Handling**: Supports custom HTTP methods, query parameters, and request parsing.
- **Cookie Management**: Efficient cookie parsing and support for managing session data.
- **Error Handling**: Defined HTTP error classes for custom exception handling.
- **Routing**: Custom routing for handling requests and responses.
- **Custom Protocol**: Built-in support for custom protocols and data serialization.

## Table of Contents
- (Usage)[#usage]
- (Configuration)[#configuration]
- (API Reference)[#api-reference]
- (License)[#license]


## Usage

1. Clone the repsitory and navigate to the project directory.

```bash
git clone
cd C2Server
```

2. Use .NET SDK to build the project.

```bash
dotnet build
```

3. Run the server using the .NET CLI.

```bash
dotnet run
```

4. Server can be accessed at `8989` port.

```bash
curl -X GET http://localhost:8989/ping
```


## Configuration
- You can change the default port it binds to in the `.env` file.

- Define your routes as an async function in the `Routes.cs` file. 

```csharp
public async static Task<HTTPResponse> Ping(HTTPRequest request)
{
    return new HTTPResponse().Text("Pong!");
}
```

## API Reference

- Access request data using the `HTTPRequest` object.

```csharp
Cookie = request.Cookies["token"];
string someParameter = request.Queries["someKey"];
string body = request.Body;
string Referer = request.Headers["Referer"];
```

- Return response using `HTTPResponse` object from the handlers.

```csharp
HTTPResponse response = new HTTPResponse();

// Set cookie
var cookie = new Cookie("SessionID", "abc123")
{
    Expiration = DateTime.UtcNow.AddHours(1),
    Secure = true,
    HttpOnly = true
};
response.AddCookie(cookie);

// Send text data in response
return response.Text("Welcome to C2Server!");

// Send JSON data
return response.JSON(new {
    "status": "success",
    "message": "ready to fire"
});

// Set a different HTTP status
return response.SetStatus(HTTPStatus.Accepted).JSON(new {
    "status": "success",
    "message": "ready to fire"
});
```

## License
This project is licensed under the GNU General Public License.