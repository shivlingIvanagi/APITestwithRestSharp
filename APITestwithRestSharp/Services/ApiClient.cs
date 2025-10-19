using System;
using System.Threading.Tasks;
using RestSharp;

namespace ApiTestFramework.Services
{
    public class ApiClient
    {
        private readonly RestClient _client;
        private const string BaseUrl = "https://jsonplaceholder.typicode.com";

        public ApiClient()
        {
            var options = new RestClientOptions(BaseUrl)
            {
                MaxTimeout = 30000
            };
            _client = new RestClient(options);
        }

        public RestResponse<T> ExecuteRequest<T>(RestRequest request)
        {
            Console.WriteLine($"Executing {request.Method} request to {request.Resource}");
            var response = _client.Execute<T>(request);
            Console.WriteLine($"Response Status: {response.StatusCode}");
            return response;
        }

        public async Task<RestResponse<T>> ExecuteRequestAsync<T>(RestRequest request)
        {
            Console.WriteLine($"Executing async {request.Method} request to {request.Resource}");
            var response = await _client.ExecuteAsync<T>(request);
            Console.WriteLine($"Response Status: {response.StatusCode}");
            return response;
        }
    }
}
