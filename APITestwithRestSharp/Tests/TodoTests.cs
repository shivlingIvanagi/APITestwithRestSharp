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
    [Category("Todos")]
    public class TodoTests : BaseTest
    {
        private TodoService _todoService;

        [SetUp]
        public new void Setup()
        {
            base.Setup();
            _todoService = new TodoService();
        }

        [Test]
        [Category("Smoke")]
        [Description("Verify GET all todos returns 200 todos")]
        public void GetAllTodos_ShouldReturn200Todos()
        {
            // Act
            var response = _todoService.GetAllTodos();

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.EqualTo(200));
            Console.WriteLine($"Retrieved {response.Data.Count} todos");
        }

        [Test]
        [Category("Smoke")]
        [TestCase(1)]
        [TestCase(100)]
        [TestCase(200)]
        [Description("Verify GET todo by ID returns correct todo")]
        public void GetTodoById_ValidId_ShouldReturnTodo(int todoId)
        {
            // Act
            var response = _todoService.GetTodoById(todoId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.EqualTo(todoId));
            Assert.That(response.Data.Title, Is.Not.Empty);
            Console.WriteLine($"Todo ID: {response.Data.Id}, Completed: {response.Data.Completed}");
        }

        [Test]
        [Category("Filter")]
        [TestCase(1)]
        [TestCase(5)]
        [Description("Verify GET todos by user ID returns user's todos")]
        public void GetTodosByUserId_ValidUserId_ShouldReturnTodos(int userId)
        {
            // Act
            var response = _todoService.GetTodosByUserId(userId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Count, Is.GreaterThan(0));
            Assert.That(response.Data.All(t => t.UserId == userId), Is.True);
            Console.WriteLine($"User {userId} has {response.Data.Count} todos");
        }

        [Test]
        [Category("Create")]
        [Description("Verify POST creates a new todo")]
        public void CreateTodo_ValidData_ShouldCreateTodo()
        {
            // Arrange
            var newTodo = TestDataGenerator.GenerateRandomTodo();

            // Act
            var response = _todoService.CreateTodo(newTodo);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.Data, Is.Not.Null);
            Assert.That(response.Data.Id, Is.GreaterThan(0));
            Console.WriteLine($"Created todo with ID: {response.Data.Id}");
        }

        [Test]
        [Category("Update")]
        [Description("Verify PUT updates an existing todo")]
        public void UpdateTodo_ValidData_ShouldUpdateTodo()
        {
            // Arrange
            int todoId = 1;
            var updatedTodo = TestDataGenerator.GenerateRandomTodo();
            updatedTodo.Id = todoId;

            // Act
            var response = _todoService.UpdateTodo(todoId, updatedTodo);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data, Is.Not.Null);
            Console.WriteLine($"Updated todo {todoId}");
        }

        [Test]
        [Category("Delete")]
        [Description("Verify DELETE removes a todo")]
        public void DeleteTodo_ValidId_ShouldDeleteTodo()
        {
            // Arrange
            int todoId = 1;

            // Act
            var response = _todoService.DeleteTodo(todoId);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Console.WriteLine($"Deleted todo {todoId}");
        }

        [Test]
        [Category("Validation")]
        [Description("Verify todo has boolean completed field")]
        public void GetTodo_ShouldHaveBooleanCompletedField()
        {
            // Act
            var response = _todoService.GetTodoById(1);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Data.Completed, Is.TypeOf<bool>());
            Console.WriteLine($"Todo completed status: {response.Data.Completed}");
        }
    }
}
