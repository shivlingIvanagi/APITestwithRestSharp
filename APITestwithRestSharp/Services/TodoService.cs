using System.Collections.Generic;
using ApiTestFramework.Models;
using RestSharp;

namespace ApiTestFramework.Services
{
    public class TodoService
    {
        private readonly ApiClient _apiClient;

        public TodoService()
        {
            _apiClient = new ApiClient();
        }

        public RestResponse<List<Todo>> GetAllTodos()
        {
            var request = new RestRequest("/todos", Method.Get);
            return _apiClient.ExecuteRequest<List<Todo>>(request);
        }

        public RestResponse<Todo> GetTodoById(int todoId)
        {
            var request = new RestRequest($"/todos/{todoId}", Method.Get);
            return _apiClient.ExecuteRequest<Todo>(request);
        }

        public RestResponse<List<Todo>> GetTodosByUserId(int userId)
        {
            var request = new RestRequest("/todos", Method.Get);
            request.AddQueryParameter("userId", userId.ToString());
            return _apiClient.ExecuteRequest<List<Todo>>(request);
        }

        public RestResponse<Todo> CreateTodo(Todo todo)
        {
            var request = new RestRequest("/todos", Method.Post);
            request.AddJsonBody(todo);
            return _apiClient.ExecuteRequest<Todo>(request);
        }

        public RestResponse<Todo> UpdateTodo(int todoId, Todo todo)
        {
            var request = new RestRequest($"/todos/{todoId}", Method.Put);
            request.AddJsonBody(todo);
            return _apiClient.ExecuteRequest<Todo>(request);
        }

        public RestResponse DeleteTodo(int todoId)
        {
            var request = new RestRequest($"/todos/{todoId}", Method.Delete);
            return _apiClient.ExecuteRequest<Todo>(request);
        }
    }
}
