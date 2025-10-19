using System.Collections.Generic;
using ApiTestFramework.Models;
using RestSharp;

namespace ApiTestFramework.Services
{
    public class CommentService
    {
        private readonly ApiClient _apiClient;

        public CommentService()
        {
            _apiClient = new ApiClient();
        }

        public RestResponse<List<Comment>> GetAllComments()
        {
            var request = new RestRequest("/comments", Method.Get);
            return _apiClient.ExecuteRequest<List<Comment>>(request);
        }

        public RestResponse<Comment> GetCommentById(int commentId)
        {
            var request = new RestRequest($"/comments/{commentId}", Method.Get);
            return _apiClient.ExecuteRequest<Comment>(request);
        }

        public RestResponse<List<Comment>> GetCommentsByPostId(int postId)
        {
            var request = new RestRequest("/comments", Method.Get);
            request.AddQueryParameter("postId", postId.ToString());
            return _apiClient.ExecuteRequest<List<Comment>>(request);
        }

        public RestResponse<Comment> CreateComment(Comment comment)
        {
            var request = new RestRequest("/comments", Method.Post);
            request.AddJsonBody(comment);
            return _apiClient.ExecuteRequest<Comment>(request);
        }

        public RestResponse<Comment> UpdateComment(int commentId, Comment comment)
        {
            var request = new RestRequest($"/comments/{commentId}", Method.Put);
            request.AddJsonBody(comment);
            return _apiClient.ExecuteRequest<Comment>(request);
        }

        public RestResponse DeleteComment(int commentId)
        {
            var request = new RestRequest($"/comments/{commentId}", Method.Delete);
            return _apiClient.ExecuteRequest<Comment>(request);
        }
    }
}
