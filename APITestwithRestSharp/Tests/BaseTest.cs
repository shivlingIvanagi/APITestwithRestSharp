using System;
using NUnit.Framework;

namespace ApiTestFramework.Tests
{
    [TestFixture]
    public class BaseTest
    {
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            Console.WriteLine("==================================================");
            Console.WriteLine("Test Suite Started: JSONPlaceholder API Tests");
            Console.WriteLine($"Test Run Time: {DateTime.Now}");
            Console.WriteLine("==================================================");
        }

        [SetUp]
        public void Setup()
        {
            Console.WriteLine($"\n[{DateTime.Now:HH:mm:ss}] Starting Test: {TestContext.CurrentContext.Test.Name}");
        }

        [TearDown]
        public void TearDown()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var testName = TestContext.CurrentContext.Test.Name;

            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Test Completed: {testName} - Status: {status}");

            if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                Console.WriteLine($"Error: {TestContext.CurrentContext.Result.Message}");
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Console.WriteLine("\n==================================================");
            Console.WriteLine("Test Suite Completed");
            Console.WriteLine("==================================================");
        }
    }
}
