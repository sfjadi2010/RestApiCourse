namespace Movies.Contracts.Requests
{
    public class CreateTokenRequest
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
        public required string Email { get; init; }
        public Dictionary<string, object> CustomClaims { get; set; } = new();
    }
}
