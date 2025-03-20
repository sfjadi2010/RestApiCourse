using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Movies.Api.Controllers
{
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TokenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost(ApiEndpoints.Token.Create)]
        public IActionResult Create([FromBody] CreateTokenRequest request, CancellationToken cancellationToken)
        {
            // Claims list
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, request.Username),
                new(JwtRegisteredClaimNames.Email, request.Username),
                new("username", request.Username),
            };

            // handle custom claims
            foreach (var claimPair in request.CustomClaims)
            {
                var jsonElement = (JsonElement)claimPair.Value;
                var valueType = jsonElement.ValueKind switch
                {
                    JsonValueKind.True => ClaimValueTypes.Boolean,
                    JsonValueKind.False => ClaimValueTypes.Boolean,
                    JsonValueKind.Number => ClaimValueTypes.Double,
                    _ => ClaimValueTypes.String
                };

                var claim = new Claim(claimPair.Key, jsonElement.ToString(), valueType);
                claims.Add(claim);
            }

            // Create the JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                 issuer: _configuration["Jwt:Issuer"],
                 audience: _configuration["Jwt:Audience"],
                 claims: claims,
                 expires: DateTime.UtcNow.AddMinutes(30),
                 signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // Generate a refresh token (this is just an example using a GUID)
            var refreshToken = Guid.NewGuid().ToString();

            // Build the response
            var response = new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Username = request.Username
            };

            return Ok(response);
        }
    }
}
