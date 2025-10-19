using System.Collections.Generic;
using ApiTestFramework.Models;
using RestSharp;

namespace ApiTestFramework.Services
{
    public class UserService
    {
        private readonly ApiClient _apiClient;

        public UserService()
        {
            _apiClient = new ApiClient();
        }

        public RestResponse<List<User>> GetAllUsers()
        {
            var request = new RestRequest("/users", Method.Get);
            return _apiClient.ExecuteRequest<List<User>>(request);
        }

        public RestResponse<User> GetUserById(int userId)
        {
            var request = new RestRequest($"/users/{userId}", Method.Get);
            return _apiClient.ExecuteRequest<User>(request);
        }

        public RestResponse<User> CreateUser(User user)
        {
            var request = new RestRequest("/users", Method.Post);
            request.AddJsonBody(user);
            return _apiClient.ExecuteRequest<User>(request);
        }

        public RestResponse<User> UpdateUser(int userId, User user)
        {
            var request = new RestRequest($"/users/{userId}", Method.Put);
            request.AddJsonBody(user);
            return _apiClient.ExecuteRequest<User>(request);
        }

        public RestResponse DeleteUser(int userId)
        {
            var request = new RestRequest($"/users/{userId}", Method.Delete);
            return _apiClient.ExecuteRequest<User>(request);
        }
    }
}
