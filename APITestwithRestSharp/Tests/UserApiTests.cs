using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ApiTestFramework.Config;
using ApiTestFramework.Helpers;
using ApiTestFramework.Models.Request;
using ApiTestFramework.Models.Response;
using NUnit.Framework;
using RestSharp;

namespace ApiTestFramework.Tests
{
    [TestFixture]
    public class UserApiTests
    {
        private ApiClient _apiClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            TestContext.WriteLine("Starting API Test Suite");
        }

        [SetUp]
        public void SetUp()
        {
            _apiClient = new ApiClient(TestConfig.BaseUrl, TestConfig.ApiKey);
            TestContext.WriteLine($"Test: {TestContext.CurrentContext.Test.Name}");
        }

        [TearDown]
        public void TearDown()
        {
            TestContext.WriteLine($"Test Status: {TestContext.CurrentContext.Result.Outcome.Status}");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            TestContext.WriteLine("API Test Suite Completed");
        }

        [Test]
        [Category("Smoke")]
        [Description("Verify that GET all users endpoint returns success")]
        public async Task GetAllUsers_ShouldReturnSuccess()
        {
            // Arrange
            var request = _apiClient.CreateRequest("/users", Method.Get);

            // Act
            var response = await _apiClient.ExecuteAsync<List<UserResponse>>(request);

            // Assert
            _apiClient.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data, Is.Not.Empty);
            Assert.That(response.Data.Count, Is.GreaterThan(0));
        }

        [Test]
        [Category("Smoke")]
        [Description("Verify that GET user by ID returns correct user")]
        public async Task GetUserById_ShouldReturnUser()
        {
            // Arrange
            int userId = 1;
            var request = _apiClient.CreateRequest($"/users/{userId}", Method.Get);

            // Act
            var response = await _apiClient.ExecuteAsync<UserResponse>(request);

            // Assert
            _apiClient.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(userId));
            Assert.That(response.Data.Name, Is.Not.Null);
            Assert.That(response.Data.Email, Is.Not.Null);
        }

        [Test]
        [Category("CRUD")]
        [Description("Verify that POST creates a new user")]
        public async Task CreateUser_ShouldReturnCreated()
        {
            // Arrange
            var newUser = new CreateUserRequest
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Username = "johndoe",
                Address = new AddressRequest
                {
                    Street = "123 Main St",
                    City = "New York",
                    ZipCode = "10001"
                }
            };

            var request = _apiClient.CreateRequestWithBody("/users", Method.Post, newUser);

            // Act
            var response = await _apiClient.ExecuteAsync<UserResponse>(request);

            // Assert
            _apiClient.AssertStatusCode(response, HttpStatusCode.Created);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.GreaterThan(0));
        }

        [Test]
        [Category("CRUD")]
        [Description("Verify that PUT updates an existing user")]
        public async Task UpdateUser_ShouldReturnSuccess()
        {
            // Arrange
            int userId = 1;
            var updateUser = new CreateUserRequest
            {
                Name = "Jane Doe Updated",
                Email = "jane.updated@example.com",
                Username = "janeupdated"
            };

            var request = _apiClient.CreateRequestWithBody($"/users/{userId}", Method.Put, updateUser);

            // Act
            var response = await _apiClient.ExecuteAsync<UserResponse>(request);

            // Assert
            _apiClient.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(response.Data, Is.Not.Null);
        }

        [Test]
        [Category("CRUD")]
        [Description("Verify that DELETE removes a user")]
        public async Task DeleteUser_ShouldReturnSuccess()
        {
            // Arrange
            int userId = 1;
            var request = _apiClient.CreateRequest($"/users/{userId}", Method.Delete);

            // Act
            var response = await _apiClient.ExecuteAsync(request);

            // Assert
            _apiClient.AssertStatusCode(response, HttpStatusCode.OK);
        }

        [Test]
        [Category("Negative")]
        [Description("Verify that GET non-existent user returns 404")]
        public async Task GetNonExistentUser_ShouldReturn404()
        {
            // Arrange
            int userId = 99999;
            var request = _apiClient.CreateRequest($"/users/{userId}", Method.Get);

            // Act
            var response = await _apiClient.ExecuteAsync<UserResponse>(request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [Category("Parameterized")]
        [Description("Verify that GET user by ID works for multiple users")]
        public async Task GetUserById_WithMultipleIds_ShouldReturnUsers(int userId)
        {
            // Arrange
            var request = _apiClient.CreateRequest($"/users/{userId}", Method.Get);

            // Act
            var response = await _apiClient.ExecuteAsync<UserResponse>(request);

            // Assert
            _apiClient.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(userId));
        }

        [Test]
        [Category("Performance")]
        [Description("Verify that API response time is acceptable")]
        public async Task GetAllUsers_ShouldRespondWithinAcceptableTime()
        {
            // Arrange
            var request = _apiClient.CreateRequest("/users", Method.Get);
            var maxResponseTime = TimeSpan.FromSeconds(5);

            // Act
            var startTime = DateTime.Now;
            var response = await _apiClient.ExecuteAsync<List<UserResponse>>(request);
            var endTime = DateTime.Now;
            var duration = endTime - startTime;

            // Assert
            _apiClient.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.That(duration, Is.LessThan(maxResponseTime),
                $"Response time {duration.TotalSeconds}s exceeded maximum {maxResponseTime.TotalSeconds}s");
        }

        [Test]
        [Category("Validation")]
        [Description("Verify user response contains all required fields")]
        public async Task GetUserById_ShouldContainAllRequiredFields()
        {
            // Arrange
            int userId = 1;
            var request = _apiClient.CreateRequest($"/users/{userId}", Method.Get);

            // Act
            var response = await _apiClient.ExecuteAsync<UserResponse>(request);

            // Assert
            _apiClient.AssertStatusCode(response, HttpStatusCode.OK);
            Assert.Multiple(() =>
            {
                Assert.That(response.Data, Is.Not.Null);
                Assert.That(response.Data.Id, Is.GreaterThan(0));
                Assert.That(response.Data.Name, Is.Not.Null.And.Not.Empty);
                Assert.That(response.Data.Username, Is.Not.Null.And.Not.Empty);
                Assert.That(response.Data.Email, Is.Not.Null.And.Not.Empty);
                Assert.That(response.Data.Email, Does.Contain("@"));
            });
        }
    }
}
