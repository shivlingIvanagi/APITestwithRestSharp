namespace ApiTestFramework.Models.Response
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public AddressResponse? Address { get; set; }
        public string? Phone { get; set; }
        public string? Website { get; set; }
    }

    public class AddressResponse
    {
        public string? Street { get; set; }
        public string? Suite { get; set; }
        public string? City { get; set; }
        public string? Zipcode { get; set; }
        public GeoResponse? Geo { get; set; }
    }

    public class GeoResponse
    {
        public string? Lat { get; set; }
        public string? Lng { get; set; }
    }
}