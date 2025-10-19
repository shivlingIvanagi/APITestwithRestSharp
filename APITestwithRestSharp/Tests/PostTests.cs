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
    [Category("Posts")]
    public class PostTests : BaseTest
    {
        private PostService _postService;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _postService = new PostService();
        }

        [Test]
        [Category("Smoke")]
        [Description("Verify GET all posts returns 100 posts")]
        public void GetAllPosts_ShouldReturn100Posts()
        {
            // Act
            var response = _postService.GetAllPosts();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.EqualTo(100));
            Console.WriteLine($"Retrieved {response.Data.Count} posts");
        }

        [Test]
        [Category("Smoke")]
        [TestCase(1)]
        [TestCase(50)]
        [TestCase(100)]
        [Description("Verify GET post by valid ID returns correct post")]
        public void GetPostById_ValidId_ShouldReturnPost(int postId)
        {
            // Act
            var response = _postService.GetPostById(postId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(postId));
            Assert.That(response.Data.Title, Is.Not.Empty);
            Assert.That(response.Data.Body, Is.Not.Empty);
            Console.WriteLine($"Post ID: {response.Data.Id}, Title: {response.Data.Title}");
        }

        [Test]
        [Category("Negative")]
        [Description("Verify GET post with invalid ID returns 404")]
        public void GetPostById_InvalidId_ShouldReturnNotFound()
        {
            // Act
            var response = _postService.GetPostById(99999);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Console.WriteLine("Invalid post ID handled correctly with 404");
        }

        [Test]
        [Category("Filter")]
        [TestCase(1)]
        [TestCase(5)]
        [Description("Verify GET posts by user ID returns user's posts")]
        public void GetPostsByUserId_ValidUserId_ShouldReturnUserPosts(int userId)
        {
            // Act
            var response = _postService.GetPostsByUserId(userId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.GreaterThan(0));
            Assert.That(response.Data.All(p => p.UserId == userId), Is.True);
            Console.WriteLine($"User {userId} has {response.Data.Count} posts");
        }

        [Test]
        [Category("Relationship")]
        [TestCase(1)]
        [TestCase(10)]
        [Description("Verify GET comments for post returns related comments")]
        public void GetCommentsForPost_ValidPostId_ShouldReturnComments(int postId)
        {
            // Act
            var response = _postService.GetCommentsForPost(postId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.GreaterThan(0));
            Assert.That(response.Data.All(c => c.PostId == postId), Is.True);
            Console.WriteLine($"Post {postId} has {response.Data.Count} comments");
        }

        [Test]
        [Category("Create")]
        [Description("Verify POST creates a new post")]
        public void CreatePost_ValidData_ShouldCreatePost()
        {
            // Arrange
            var newPost = TestDataGenerator.GenerateRandomPost();

            // Act
            var response = _postService.CreatePost(newPost);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Title, Is.EqualTo(newPost.Title));
            Assert.That(response.Data.Body, Is.EqualTo(newPost.Body));
            Assert.That(response.Data.Id, Is.GreaterThan(0));
            Console.WriteLine($"Created post with ID: {response.Data.Id}");
        }

        [Test]
        [Category("Update")]
        [Description("Verify PUT updates an existing post")]
        public void UpdatePost_ValidData_ShouldUpdatePost()
        {
            // Arrange
            int postId = 1;
            var updatedPost = TestDataGenerator.GenerateRandomPost();
            updatedPost.Id = postId;

            // Act
            var response = _postService.UpdatePost(postId, updatedPost);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(postId));
            Console.WriteLine($"Updated post {postId}");
        }

        [Test]
        [Category("Update")]
        [Description("Verify PATCH partially updates a post")]
        public void PatchPost_ValidData_ShouldPartiallyUpdatePost()
        {
            // Arrange
            int postId = 1;
            var partialUpdate = new { title = "Updated Title" };

            // Act
            var response = _postService.PatchPost(postId, partialUpdate);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Console.WriteLine($"Patched post {postId}");
        }

        [Test]
        [Category("Delete")]
        [Description("Verify DELETE removes a post")]
        public void DeletePost_ValidId_ShouldDeletePost()
        {
            // Arrange
            int postId = 1;

            // Act
            var response = _postService.DeletePost(postId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Console.WriteLine($"Deleted post {postId}");
        }
    }
}
