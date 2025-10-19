using System;
using System.Net;
using ApiTestFramework.Services;
using ApiTestFramework.Utilities;
using NUnit.Framework;

namespace ApiTestFramework.Tests
{
    [TestFixture]
    [Category("Users")]
    [Parallelizable(ParallelScope.All)]
    public class UserTests : BaseTest
    {
        private UserService _userService;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _userService = new UserService();
        }

        [Test]
        [Category("Smoke")]
        [Description("Verify GET all users returns 10 users")]
        public void GetAllUsers_ShouldReturn10Users()
        {
            // Act
            var response = _userService.GetAllUsers();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.EqualTo(10));
            Console.WriteLine($"Retrieved {response.Data.Count} users");
        }

        [Test]
        [Category("Smoke")]
        [TestCase(1)]
        [TestCase(5)]
        [TestCase(10)]
        [Description("Verify GET user by ID returns correct user")]
        public void GetUserById_ValidId_ShouldReturnUser(int userId)
        {
            // Act
            var response = _userService.GetUserById(userId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(userId));
            Assert.That(response.Data.Name, Is.Not.Empty);
            Assert.That(response.Data.Email, Does.Contain("@"));
            Console.WriteLine($"User ID: {response.Data.Id}, Name: {response.Data.Name}");
        }

        [Test]
        [Category("Negative")]
        [Description("Verify GET user with invalid ID returns 404")]
        public void GetUserById_InvalidId_ShouldReturnNotFound()
        {
            // Act
            var response = _userService.GetUserById(99999);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Console.WriteLine("Invalid user ID handled correctly with 404");
        }

        [Test]
        [Category("Create")]
        [Description("Verify POST creates a new user")]
        public void CreateUser_ValidData_ShouldCreateUser()
        {
            // Arrange
            var newUser = TestDataGenerator.GenerateRandomUser();

            // Act
            var response = _userService.CreateUser(newUser);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.GreaterThan(0));
            Console.WriteLine($"Created user with ID: {response.Data.Id}");
        }

        [Test]
        [Category("Update")]
        [Description("Verify PUT updates an existing user")]
        public void UpdateUser_ValidData_ShouldUpdateUser()
        {
            // Arrange
            int userId = 1;
            var updatedUser = TestDataGenerator.GenerateRandomUser();
            updatedUser.Id = userId;

            // Act
            var response = _userService.UpdateUser(userId, updatedUser);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Console.WriteLine($"Updated user {userId}");
        }

        [Test]
        [Category("Delete")]
        [Description("Verify DELETE removes a user")]
        public void DeleteUser_ValidId_ShouldDeleteUser()
        {
            // Arrange
            int userId = 1;

            // Act
            var response = _userService.DeleteUser(userId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Console.WriteLine($"Deleted user {userId}");
        }

        [Test]
        [Category("Validation")]
        [Description("Verify user has complete address information")]
        public void GetUser_ShouldHaveCompleteAddressInfo()
        {
            // Act
            var response = _userService.GetUserById(1);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data.Address, Is.Not.Null);
            Assert.That(response.Data.Address.Street, Is.Not.Empty);
            Assert.That(response.Data.Address.City, Is.Not.Empty);
            Assert.That(response.Data.Address.Zipcode, Is.Not.Empty);
            Assert.That(response.Data.Address.Geo, Is.Not.Null);
            Console.WriteLine($"Address: {response.Data.Address.Street}, {response.Data.Address.City}");
        }

        [Test]
        [Category("Validation")]
        [Description("Verify user has company information")]
        public void GetUser_ShouldHaveCompanyInfo()
        {
            // Act
            var response = _userService.GetUserById(1);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data.Company, Is.Not.Null);
            Assert.That(response.Data.Company.Name, Is.Not.Empty);
            Console.WriteLine($"Company: {response.Data.Company.Name}");
        }

        [Test]
        [Category("Validation")]
        [Description("Verify user email format is valid")]
        public void GetUser_ShouldHaveValidEmailFormat()
        {
            // Act
            var response = _userService.GetUserById(1);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data.Email, Does.Match(@"^[^@\s]+@[^@\s]+\.[^@\s]+$"));
            Console.WriteLine($"Email format valid: {response.Data.Email}");
        }
    }
}
