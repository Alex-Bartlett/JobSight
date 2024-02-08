using Shared.Models;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ManagementApp.Services
{
    public enum HttpRequestMethod
    {
        GET,
        PATCH,
        PUT,
        POST,
        DELETE,
    }

    public class ApiDataService
    {
        private readonly string ApiAddress;
        public ApiDataService(IConfiguration configuration)
        {
            // Get api address from config. Throw error if not found.
            ApiAddress = configuration.GetValue<string>("API") ?? throw new ArgumentNullException(nameof(ApiAddress), "API URL is missing from configuration.");
        }

        /// <summary>
        /// Sends a request to the API to get/send data
        /// </summary>
        /// <typeparam name="TResult">Model to deserialize into</typeparam>
        /// <param name="requestMethod">The HTTP method for the request</param>
        /// <param name="relativeAddress">The relative path of the api for the request</param>
        /// <param name="body">The payload, as stringified JSON</param>
        /// <returns>The JSON response, if any</returns>
        public async Task<TResult?> SendQuery<TResult>(HttpRequestMethod requestMethod, string relativeAddress, string? body = null)
        {
            // Used the following guide: https://stackoverflow.com/a/10679340

            using HttpClient client = new();
            client.BaseAddress = new Uri(ApiAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response;

            // Send body if request is body exists and method is not 
            if (body is not null && requestMethod != HttpRequestMethod.GET)
            {
                HttpRequestMessage request = new(ToHttpMethod(requestMethod), relativeAddress);
                // Set the request content
                request.Content = new StringContent(body, Encoding.UTF8, "application/json");
                response = await client.SendAsync(request);
            }
            else
            {
                response = await client.GetAsync(relativeAddress);
            }

            string responseContent = await response.Content.ReadAsStringAsync();
            try
            {
                return JsonSerializer.Deserialize<TResult>(responseContent);
            }
            catch (JsonException)
            {
                // An error occurred. This is likely because the response did not contain JSON.
                return default(TResult);
            }
        }

        /// <summary>
        /// Converts an HttpRequestMethod (enum) to the appropriate HttpMethod (object)
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        private HttpMethod ToHttpMethod(HttpRequestMethod method)
        {
            switch (method)
            {
                case HttpRequestMethod.GET:
                    return HttpMethod.Get;
                case HttpRequestMethod.POST:
                    return HttpMethod.Post;
                case HttpRequestMethod.PUT:
                    return HttpMethod.Put;
                case HttpRequestMethod.PATCH:
                    return HttpMethod.Patch;
                case HttpRequestMethod.DELETE:
                    return HttpMethod.Delete;
                default:
                    throw new ArgumentException($"Invalid/unknown request method {method}", nameof(method));
            }
        }
    }
}
