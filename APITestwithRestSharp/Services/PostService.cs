using System.Collections.Generic;
using ApiTestFramework.Models;
using RestSharp;

namespace ApiTestFramework.Services
{
    public class PostService
    {
        private readonly ApiClient _apiClient;

        public PostService()
        {
            _apiClient = new ApiClient();
        }

        public RestResponse<List<Post>> GetAllPosts()
        {
            var request = new RestRequest("/posts", Method.Get);
            return _apiClient.ExecuteRequest<List<Post>>(request);
        }

        public RestResponse<Post> GetPostById(int postId)
        {
            var request = new RestRequest($"/posts/{postId}", Method.Get);
            return _apiClient.ExecuteRequest<Post>(request);
        }

        public RestResponse<List<Post>> GetPostsByUserId(int userId)
        {
            var request = new RestRequest("/posts", Method.Get);
            request.AddQueryParameter("userId", userId.ToString());
            return _apiClient.ExecuteRequest<List<Post>>(request);
        }

        public RestResponse<List<Comment>> GetCommentsForPost(int postId)
        {
            var request = new RestRequest($"/posts/{postId}/comments", Method.Get);
            return _apiClient.ExecuteRequest<List<Comment>>(request);
        }

        public RestResponse<Post> CreatePost(Post post)
        {
            var request = new RestRequest("/posts", Method.Post);
            request.AddJsonBody(post);
            return _apiClient.ExecuteRequest<Post>(request);
        }

        public RestResponse<Post> UpdatePost(int postId, Post post)
        {
            var request = new RestRequest($"/posts/{postId}", Method.Put);
            request.AddJsonBody(post);
            return _apiClient.ExecuteRequest<Post>(request);
        }

        public RestResponse<Post> PatchPost(int postId, object partialPost)
        {
            var request = new RestRequest($"/posts/{postId}", Method.Patch);
            request.AddJsonBody(partialPost);
            return _apiClient.ExecuteRequest<Post>(request);
        }

        public RestResponse DeletePost(int postId)
        {
            var request = new RestRequest($"/posts/{postId}", Method.Delete);
            return _apiClient.ExecuteRequest<Post>(request);
        }
    }
}
