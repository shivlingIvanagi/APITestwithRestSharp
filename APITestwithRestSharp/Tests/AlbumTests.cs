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
    [Category("Albums")]
    public class AlbumTests : BaseTest
    {
        private AlbumService _albumService;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _albumService = new AlbumService();
        }

        [Test]
        [Category("Smoke")]
        [Description("Verify GET all albums returns 100 albums")]
        public void GetAllAlbums_ShouldReturn100Albums()
        {
            // Act
            var response = _albumService.GetAllAlbums();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.EqualTo(100));
            Console.WriteLine($"Retrieved {response.Data.Count} albums");
        }

        [Test]
        [Category("Smoke")]
        [TestCase(1)]
        [TestCase(50)]
        [TestCase(100)]
        [Description("Verify GET album by ID returns correct album")]
        public void GetAlbumById_ValidId_ShouldReturnAlbum(int albumId)
        {
            // Act
            var response = _albumService.GetAlbumById(albumId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(albumId));
            Assert.That(response.Data.Title, Is.Not.Empty);
            Console.WriteLine($"Album ID: {response.Data.Id}, Title: {response.Data.Title}");
        }

        [Test]
        [Category("Filter")]
        [TestCase(1)]
        [TestCase(5)]
        [Description("Verify GET albums by user ID returns user's albums")]
        public void GetAlbumsByUserId_ValidUserId_ShouldReturnAlbums(int userId)
        {
            // Act
            var response = _albumService.GetAlbumsByUserId(userId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.GreaterThan(0));
            Assert.That(response.Data.All(a => a.UserId == userId), Is.True);
            Console.WriteLine($"User {userId} has {response.Data.Count} albums");
        }

        [Test]
        [Category("Relationship")]
        [TestCase(1)]
        [TestCase(5)]
        [Description("Verify GET photos for album returns related photos")]
        public void GetPhotosForAlbum_ValidAlbumId_ShouldReturnPhotos(int albumId)
        {
            // Act
            var response = _albumService.GetPhotosForAlbum(albumId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.GreaterThan(0));
            Assert.That(response.Data.All(p => p.AlbumId == albumId), Is.True);
            Console.WriteLine($"Album {albumId} has {response.Data.Count} photos");
        }

        [Test]
        [Category("Create")]
        [Description("Verify POST creates a new album")]
        public void CreateAlbum_ValidData_ShouldCreateAlbum()
        {
            // Arrange
            var newAlbum = TestDataGenerator.GenerateRandomAlbum();

            // Act
            var response = _albumService.CreateAlbum(newAlbum);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.GreaterThan(0));
            Console.WriteLine($"Created album with ID: {response.Data.Id}");
        }

        [Test]
        [Category("Update")]
        [Description("Verify PUT updates an existing album")]
        public void UpdateAlbum_ValidData_ShouldUpdateAlbum()
        {
            // Arrange
            int albumId = 1;
            var updatedAlbum = TestDataGenerator.GenerateRandomAlbum();
            updatedAlbum.Id = albumId;

            // Act
            var response = _albumService.UpdateAlbum(albumId, updatedAlbum);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Console.WriteLine($"Updated album {albumId}");
        }

        [Test]
        [Category("Delete")]
        [Description("Verify DELETE removes an album")]
        public void DeleteAlbum_ValidId_ShouldDeleteAlbum()
        {
            // Arrange
            int albumId = 1;

            // Act
            var response = _albumService.DeleteAlbum(albumId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Console.WriteLine($"Deleted album {albumId}");
        }
    }
}
