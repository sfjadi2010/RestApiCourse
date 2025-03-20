namespace Movies.Contracts.Responses
{
    public class TokenResponse
    {
        public required string AccessToken { get; init; }
        public required string RefreshToken { get; init; }
        public required string Username { get; init; }
    }
}
