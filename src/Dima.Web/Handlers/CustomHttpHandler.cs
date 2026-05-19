using System.Net;
using MudBlazor;

namespace Dima.Web.Handlers;

public class CustomHttpHandler(ISnackbar snackbar, ILogger<CustomHttpHandler> logger)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        HttpResponseMessage? response = null;

        try
        {
            response = await base.SendAsync(request, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            logger.LogError(
                ex,
                "Network error on {Method} {Url}",
                request.Method,
                request.RequestUri
            );
            snackbar.Add("Unable to connect to the server. Check your connection.", Severity.Error);
            return new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
        }

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            logger.LogWarning(
                "HTTP {StatusCode} on {Method} {Url} — {Body}",
                (int)response.StatusCode,
                request.Method,
                request.RequestUri,
                body
            );

            var message = response.StatusCode switch
            {
                HttpStatusCode.Unauthorized => "You must be logged in to perform this action.",
                HttpStatusCode.Forbidden => "You do not have permission to perform this action.",
                HttpStatusCode.NotFound => "The requested resource was not found.",
                HttpStatusCode.UnprocessableEntity => "Invalid data. Please check your input.",
                HttpStatusCode.TooManyRequests => "Too many requests. Please try again later.",
                HttpStatusCode.InternalServerError => "An internal server error occurred.",
                _ => $"Unexpected error ({(int)response.StatusCode}).",
            };

            snackbar.Add(message, Severity.Error);
        }

        return response;
    }
}
