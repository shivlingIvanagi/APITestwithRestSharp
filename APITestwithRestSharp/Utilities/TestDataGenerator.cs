using System;
using ApiTestFramework.Models;

namespace ApiTestFramework.Utilities
{
    public static class TestDataGenerator
    {
        private static readonly Random Random = new Random();

        public static Post GenerateRandomPost()
        {
            return new Post
            {
                UserId = Random.Next(1, 11),
                Title = $"Test Post Title {Random.Next(1000, 9999)}",
                Body = $"Test post body content {Random.Next(1000, 9999)}"
            };
        }

        public static Comment GenerateRandomComment()
        {
            return new Comment
            {
                PostId = Random.Next(1, 100),
                Name = $"Test Comment {Random.Next(1000, 9999)}",
                Email = $"test{Random.Next(1000, 9999)}@example.com",
                Body = $"Test comment body {Random.Next(1000, 9999)}"
            };
        }

        public static Album GenerateRandomAlbum()
        {
            return new Album
            {
                UserId = Random.Next(1, 11),
                Title = $"Test Album {Random.Next(1000, 9999)}"
            };
        }

        public static Photo GenerateRandomPhoto()
        {
            return new Photo
            {
                AlbumId = Random.Next(1, 100),
                Title = $"Test Photo {Random.Next(1000, 9999)}",
                Url = $"https://via.placeholder.com/600/{Random.Next(100, 999)}",
                ThumbnailUrl = $"https://via.placeholder.com/150/{Random.Next(100, 999)}"
            };
        }

        public static Todo GenerateRandomTodo()
        {
            return new Todo
            {
                UserId = Random.Next(1, 11),
                Title = $"Test Todo {Random.Next(1000, 9999)}",
                Completed = Random.Next(0, 2) == 1
            };
        }

        public static User GenerateRandomUser()
        {
            return new User
            {
                Name = $"Test User {Random.Next(1000, 9999)}",
                Username = $"testuser{Random.Next(1000, 9999)}",
                Email = $"user{Random.Next(1000, 9999)}@example.com",
                Phone = GeneratePhoneNumber(),
                Website = "example.com",
                Address = new Address
                {
                    Street = $"{Random.Next(1, 999)} Test St",
                    Suite = $"Apt. {Random.Next(1, 100)}",
                    City = "TestCity",
                    Zipcode = GenerateZipCode(),
                    Geo = new Geo
                    {
                        Lat = Random.Next(-90, 90).ToString(),
                        Lng = Random.Next(-180, 180).ToString()
                    }
                },
                Company = new Company
                {
                    Name = $"Test Company {Random.Next(100, 999)}",
                    CatchPhrase = "Testing Excellence",
                    Bs = "quality assurance"
                }
            };
        }

        private static string GeneratePhoneNumber()
        {
            return $"{Random.Next(100, 999)}-{Random.Next(100, 999)}-{Random.Next(1000, 9999)}";
        }

        private static string GenerateZipCode()
        {
            return $"{Random.Next(10000, 99999)}";
        }
    }
}
