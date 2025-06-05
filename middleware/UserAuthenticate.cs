using System.IdentityModel.Tokens.Jwt;
using Clerk.BackendAPI.Helpers.Jwks;
using System.Net.Http;

namespace webbuilder.api.middleware
{
    public class UserAuthenticate
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;

        public UserAuthenticate(RequestDelegate next, IHttpClientFactory httpClientFactory)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var request = new HttpRequestMessage
            {
                Method = new HttpMethod(context.Request.Method),
                RequestUri = new Uri($"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}"),
                Content = new StreamContent(context.Request.Body)
            };

            if (context.Request.Headers.ContainsKey("Authorization"))
            {
                request.Headers.Authorization = System.Net.Http.Headers.AuthenticationHeaderValue.Parse(
                    context.Request.Headers["Authorization"].ToString()
                );
            }

            foreach (var header in context.Request.Headers)
            {
                if (!request.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    request.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            bool isSignedIn = await IsSignedInAsync(request);
            if (!isSignedIn)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }
            else
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(request?.Headers?.Authorization?.Parameter);
                var userId = token.Claims.First(claim => claim.Type == "sub").Value;
                context.Items["userId"] = userId;
            }

            await _next(context);
        }

        public async Task<bool> IsSignedInAsync(HttpRequestMessage request)
        {
            try
            {
                // Use the configured HttpClient for better SSL/TLS handling
                var options = new AuthenticateRequestOptions(
                    secretKey: Environment.GetEnvironmentVariable("CLERK_SECRET_KEY"),
                    authorizedParties: new string[] { "http://localhost:3000" }
                );
                // Pass the HttpClient as a separate parameter if supported, or use the default client
                var requestState = await AuthenticateRequest.AuthenticateRequestAsync(request, options);
                return requestState.IsSignedIn();
            }
            catch (HttpRequestException ex)
            {
                // Log the error (consider adding proper logging)
                Console.WriteLine($"Authentication error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return false;
            }
        }
    }

    public static class AuthenticateRequestExtension
    {
        public static IApplicationBuilder UseUserAuthenticate(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserAuthenticate>();
        }
    }
}