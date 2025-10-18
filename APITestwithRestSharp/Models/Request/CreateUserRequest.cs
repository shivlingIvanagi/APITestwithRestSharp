namespace ApiTestFramework.Models.Request
{
    public class CreateUserRequest
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public AddressRequest? Address { get; set; }
    }

    public class AddressRequest
    {
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? ZipCode { get; set; }
    }
}