using System;
using System.Net;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace ApiTestFramework.Helpers
{
    public class ApiClient
    {
        private readonly RestClient _client;

        public ApiClient(string baseUrl, string? apiKey = null)
        {
            var options = new RestClientOptions(baseUrl)
            {
                MaxTimeout = Config.TestConfig.TimeoutSeconds * 1000
            };

            if (!string.IsNullOrEmpty(apiKey))
            {
                options.Authenticator = new JwtAuthenticator(apiKey);
            }

            _client = new RestClient(options);
        }

        public async Task<RestResponse<T>> ExecuteAsync<T>(RestRequest request)
        {
            var response = await _client.ExecuteAsync<T>(request);
            LogRequest(request, response);
            return response;
        }

        public async Task<RestResponse> ExecuteAsync(RestRequest request)
        {
            var response = await _client.ExecuteAsync(request);
            LogRequest(request, response);
            return response;
        }

        public RestRequest CreateRequest(string resource, Method method)
        {
            var request = new RestRequest(resource, method);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            return request;
        }

        public RestRequest CreateRequestWithBody<T>(string resource, Method method, T body)
        {
            var request = CreateRequest(resource, method);
            request.AddJsonBody(new { value = body });
            return request;
        }

        private void LogRequest(RestRequest request, RestResponse response)
        {
            Console.WriteLine($"\n--- API Request ---");
            Console.WriteLine($"URL: {_client.BuildUri(request)}");
            Console.WriteLine($"Method: {request.Method}");
            Console.WriteLine($"Status Code: {(int)response.StatusCode} ({response.StatusCode})");
            Console.WriteLine($"Response Time: {response}ms");

            if (!string.IsNullOrEmpty(response.Content))
            {
                Console.WriteLine($"Response: {response.Content?.Substring(0, Math.Min(200, response.Content?.Length ?? 0))}...");
            }
            Console.WriteLine("-------------------\n");
        }

        public void AssertStatusCode(RestResponse response, HttpStatusCode expectedCode)
        {
            if (response.StatusCode != expectedCode)
            {
                throw new Exception($"Expected status code {expectedCode} but got {response.StatusCode}. Response: {response.Content}");
            }
        }
    }
}
