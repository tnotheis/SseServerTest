using System.Text;

var app = WebApplication.CreateBuilder(args).Build();

app.MapGet("/sse", async context =>
    {
        context.Response.StatusCode = 200;
        context.Response.Headers.CacheControl = "no-cache";
        context.Response.Headers.Connection = "keep-alive";
        context.Response.Headers.ContentType = "text/event-stream";

        var i = 1;

        while (!context.RequestAborted.IsCancellationRequested)
        {
            await Task.Delay(1000);
            var formattedMessage = $"data: {i++}\n\n";
            var messageBytes = Encoding.UTF8.GetBytes(formattedMessage);
            await context.Response.Body.WriteAsync(messageBytes);
            await context.Response.Body.FlushAsync();
        }
    });

app.MapGet("/health", () => "OK");

app.Run();
