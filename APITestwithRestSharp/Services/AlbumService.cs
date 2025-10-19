using System.Collections.Generic;
using ApiTestFramework.Models;
using RestSharp;

namespace ApiTestFramework.Services
{
    public class AlbumService
    {
        private readonly ApiClient _apiClient;

        public AlbumService()
        {
            _apiClient = new ApiClient();
        }

        public RestResponse<List<Album>> GetAllAlbums()
        {
            var request = new RestRequest("/albums", Method.Get);
            return _apiClient.ExecuteRequest<List<Album>>(request);
        }

        public RestResponse<Album> GetAlbumById(int albumId)
        {
            var request = new RestRequest($"/albums/{albumId}", Method.Get);
            return _apiClient.ExecuteRequest<Album>(request);
        }

        public RestResponse<List<Album>> GetAlbumsByUserId(int userId)
        {
            var request = new RestRequest("/albums", Method.Get);
            request.AddQueryParameter("userId", userId.ToString());
            return _apiClient.ExecuteRequest<List<Album>>(request);
        }

        public RestResponse<List<Photo>> GetPhotosForAlbum(int albumId)
        {
            var request = new RestRequest($"/albums/{albumId}/photos", Method.Get);
            return _apiClient.ExecuteRequest<List<Photo>>(request);
        }

        public RestResponse<Album> CreateAlbum(Album album)
        {
            var request = new RestRequest("/albums", Method.Post);
            request.AddJsonBody(album);
            return _apiClient.ExecuteRequest<Album>(request);
        }

        public RestResponse<Album> UpdateAlbum(int albumId, Album album)
        {
            var request = new RestRequest($"/albums/{albumId}", Method.Put);
            request.AddJsonBody(album);
            return _apiClient.ExecuteRequest<Album>(request);
        }

        public RestResponse DeleteAlbum(int albumId)
        {
            var request = new RestRequest($"/albums/{albumId}", Method.Delete);
            return _apiClient.ExecuteRequest<Album>(request);
        }
    }
}
