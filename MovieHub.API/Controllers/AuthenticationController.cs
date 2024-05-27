using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace MovieHub.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion(1)]
public class AuthenticationController(IConfiguration configuration) : ControllerBase
{
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

    public class AuthenticationRequestBody(string? username, string? password)
    {
        public string? Username { get; } = username;
        public string? Password { get; } = password;
    }

    private class MovieHubUser(int userId, string username, string firstname, string lastname)
    {
        public int UserId { get; } = userId;
        public string Username { get; set; } = username;
        public string FirstName { get; } = firstname;
        public string LastName { get; } = lastname;
    }

    [HttpPost("authenticate")]
    [Produces("text/plain")]
    [Consumes("application/json")]
    public ActionResult<string> Authenticate([FromBody] AuthenticationRequestBody requestBody)
    {
        var secretKey = _configuration["Authentication:SecretKey"] ??
                        throw new ArgumentNullException(nameof(Authenticate));

        var user = ValidateUserCredentials(
            requestBody.Username,
            requestBody.Password
        );

        if (user == null) return Unauthorized();

        var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("sub", user.UserId.ToString()),
            new("given_name", user.FirstName),
            new("family_name", user.LastName),
        };

        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["Authentication:Issuer"],
            _configuration["Authentication:Audience"],
            claims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        
        return Ok(token);
    }

    private static MovieHubUser? ValidateUserCredentials(string? username, string? password)
    {
        if (username != null && password != null) return new MovieHubUser(
            userId: 1, 
            username: "username",
            firstname: "Test", 
            lastname: "Test"
        );

        return null;
    }
}