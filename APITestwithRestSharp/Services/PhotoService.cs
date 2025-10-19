using System.Collections.Generic;
using ApiTestFramework.Models;
using RestSharp;

namespace ApiTestFramework.Services
{
    public class PhotoService
    {
        private readonly ApiClient _apiClient;

        public PhotoService()
        {
            _apiClient = new ApiClient();
        }

        public RestResponse<List<Photo>> GetAllPhotos()
        {
            var request = new RestRequest("/photos", Method.Get);
            return _apiClient.ExecuteRequest<List<Photo>>(request);
        }

        public RestResponse<Photo> GetPhotoById(int photoId)
        {
            var request = new RestRequest($"/photos/{photoId}", Method.Get);
            return _apiClient.ExecuteRequest<Photo>(request);
        }

        public RestResponse<List<Photo>> GetPhotosByAlbumId(int albumId)
        {
            var request = new RestRequest("/photos", Method.Get);
            request.AddQueryParameter("albumId", albumId.ToString());
            return _apiClient.ExecuteRequest<List<Photo>>(request);
        }

        public RestResponse<Photo> CreatePhoto(Photo photo)
        {
            var request = new RestRequest("/photos", Method.Post);
            request.AddJsonBody(photo);
            return _apiClient.ExecuteRequest<Photo>(request);
        }

        public RestResponse<Photo> UpdatePhoto(int photoId, Photo photo)
        {
            var request = new RestRequest($"/photos/{photoId}", Method.Put);
            request.AddJsonBody(photo);
            return _apiClient.ExecuteRequest<Photo>(request);
        }

        public RestResponse DeletePhoto(int photoId)
        {
            var request = new RestRequest($"/photos/{photoId}", Method.Delete);
            return _apiClient.ExecuteRequest<Photo>(request);
        }
    }
}
