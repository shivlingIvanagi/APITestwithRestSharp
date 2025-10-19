using System;
using System.Linq;
using System.Net;
using ApiTestFramework.Services;
using ApiTestFramework.Utilities;
using NUnit.Framework;

namespace ApiTestFramework.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    [Category("Photos")]
    public class PhotoTests : BaseTest
    {
        private PhotoService _photoService;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _photoService = new PhotoService();
        }

        [Test]
        [Category("Smoke")]
        [Description("Verify GET all photos returns 5000 photos")]
        public void GetAllPhotos_ShouldReturn5000Photos()
        {
            // Act
            var response = _photoService.GetAllPhotos();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.EqualTo(5000));
            Console.WriteLine($"Retrieved {response.Data.Count} photos");
        }

        [Test]
        [Category("Smoke")]
        [TestCase(1)]
        [TestCase(2500)]
        [TestCase(5000)]
        [Description("Verify GET photo by ID returns correct photo")]
        public void GetPhotoById_ValidId_ShouldReturnPhoto(int photoId)
        {
            // Act
            var response = _photoService.GetPhotoById(photoId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(photoId));
            Assert.That(response.Data.Url, Does.StartWith("https://"));
            Assert.That(response.Data.ThumbnailUrl, Does.StartWith("https://"));
            Console.WriteLine($"Photo ID: {response.Data.Id}, Title: {response.Data.Title}");
        }

        [Test]
        [Category("Filter")]
        [TestCase(1)]
        [TestCase(10)]
        [Description("Verify GET photos by album ID returns album photos")]
        public void GetPhotosByAlbumId_ValidAlbumId_ShouldReturnPhotos(int albumId)
        {
            // Act
            var response = _photoService.GetPhotosByAlbumId(albumId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.GreaterThan(0));
            Assert.That(response.Data.All(p => p.AlbumId == albumId), Is.True);
            Console.WriteLine($"Album {albumId} has {response.Data.Count} photos");
        }

        [Test]
        [Category("Create")]
        [Description("Verify POST creates a new photo")]
        public void CreatePhoto_ValidData_ShouldCreatePhoto()
        {
            // Arrange
            var newPhoto = TestDataGenerator.GenerateRandomPhoto();

            // Act
            var response = _photoService.CreatePhoto(newPhoto);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.GreaterThan(0));
            Console.WriteLine($"Created photo with ID: {response.Data.Id}");
        }

        [Test]
        [Category("Update")]
        [Description("Verify PUT updates an existing photo")]
        public void UpdatePhoto_ValidData_ShouldUpdatePhoto()
        {
            // Arrange
            int photoId = 1;
            var updatedPhoto = TestDataGenerator.GenerateRandomPhoto();
            updatedPhoto.Id = photoId;

            // Act
            var response = _photoService.UpdatePhoto(photoId, updatedPhoto);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Console.WriteLine($"Updated photo {photoId}");
        }

        [Test]
        [Category("Delete")]
        [Description("Verify DELETE removes a photo")]
        public void DeletePhoto_ValidId_ShouldDeletePhoto()
        {
            // Arrange
            int photoId = 1;

            // Act
            var response = _photoService.DeletePhoto(photoId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Console.WriteLine($"Deleted photo {photoId}");
        }

        [Test]
        [Category("Validation")]
        [Description("Verify photo URLs are valid")]
        public void GetPhoto_ShouldHaveValidUrls()
        {
            // Act
            var response = _photoService.GetPhotoById(1);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data.Url, Does.Match(@"^https?://"));
            Assert.That(response.Data.ThumbnailUrl, Does.Match(@"^https?://"));
            Console.WriteLine($"URLs valid - Main: {response.Data.Url}, Thumb: {response.Data.ThumbnailUrl}");
        }
    }
}
