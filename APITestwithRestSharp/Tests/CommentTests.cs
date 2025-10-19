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
    [Category("Comments")]
    public class CommentTests : BaseTest
    {
        private CommentService _commentService;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _commentService = new CommentService();
        }

        [Test]
        [Category("Smoke")]
        [Description("Verify GET all comments returns 500 comments")]
        public void GetAllComments_ShouldReturn500Comments()
        {
            // Act
            var response = _commentService.GetAllComments();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.EqualTo(500));
            Console.WriteLine($"Retrieved {response.Data.Count} comments");
        }

        [Test]
        [Category("Smoke")]
        [TestCase(1)]
        [TestCase(250)]
        [TestCase(500)]
        [Description("Verify GET comment by ID returns correct comment")]
        public void GetCommentById_ValidId_ShouldReturnComment(int commentId)
        {
            // Act
            var response = _commentService.GetCommentById(commentId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(commentId));
            Assert.That(response.Data.Email, Does.Contain("@"));
            Console.WriteLine($"Comment ID: {response.Data.Id}, Email: {response.Data.Email}");
        }

        [Test]
        [Category("Filter")]
        [TestCase(1)]
        [TestCase(10)]
        [Description("Verify GET comments by post ID returns post comments")]
        public void GetCommentsByPostId_ValidPostId_ShouldReturnComments(int postId)
        {
            // Act
            var response = _commentService.GetCommentsByPostId(postId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.GreaterThan(0));
            Assert.That(response.Data.All(c => c.PostId == postId), Is.True);
            Console.WriteLine($"Post {postId} has {response.Data.Count} comments");
        }

        [Test]
        [Category("Create")]
        [Description("Verify POST creates a new comment")]
        public void CreateComment_ValidData_ShouldCreateComment()
        {
            // Arrange
            var newComment = TestDataGenerator.GenerateRandomComment();

            // Act
            var response = _commentService.CreateComment(newComment);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.GreaterThan(0));
            Console.WriteLine($"Created comment with ID: {response.Data.Id}");
        }

        [Test]
        [Category("Update")]
        [Description("Verify PUT updates an existing comment")]
        public void UpdateComment_ValidData_ShouldUpdateComment()
        {
            // Arrange
            int commentId = 1;
            var updatedComment = TestDataGenerator.GenerateRandomComment();
            updatedComment.Id = commentId;

            // Act
            var response = _commentService.UpdateComment(commentId, updatedComment);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Console.WriteLine($"Updated comment {commentId}");
        }

        [Test]
        [Category("Delete")]
        [Description("Verify DELETE removes a comment")]
        public void DeleteComment_ValidId_ShouldDeleteComment()
        {
            // Arrange
            int commentId = 1;

            // Act
            var response = _commentService.DeleteComment(commentId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Console.WriteLine($"Deleted comment {commentId}");
        }

        [Test]
        [Category("Validation")]
        [Description("Verify comment email format is valid")]
        public void GetComment_ShouldHaveValidEmailFormat()
        {
            // Act
            var response = _commentService.GetCommentById(1);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data.Email, Does.Match(@"^[^@\s]+@[^@\s]+\.[^@\s]+$"));
            Console.WriteLine($"Email format valid: {response.Data.Email}");
        }
    }
}
