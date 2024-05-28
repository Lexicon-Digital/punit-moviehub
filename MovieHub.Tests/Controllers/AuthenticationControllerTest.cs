using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieHub.Controllers;
using Microsoft.Extensions.Configuration;

namespace MovieHub.Tests.Controllers;

public class AuthenticationControllerTest
{
    private readonly AuthenticationController _authenticationController;

    public AuthenticationControllerTest()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        mockConfiguration.SetupGet(configuration => configuration["Authentication:SecretKey"]).Returns("OTM4Y2ZhNWEtOTJkYi02YjFmLTM4YjgtMDFiMDc5ODVkZDhl");
        mockConfiguration.SetupGet(configuration => configuration["Authentication:Issuer"]).Returns("issuer");
        mockConfiguration.SetupGet(configuration => configuration["Authentication:Audience"]).Returns("audience");

        _authenticationController = new AuthenticationController(mockConfiguration.Object);
    }
    
    [Fact]
    public void Authenticate_Returns_JWT()
    {
        var requestBody = new AuthenticationController.AuthenticationRequestBody("username", "password");

        var result = _authenticationController.Authenticate(requestBody);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.NotNull(okResult.Value);
        Assert.IsType<string>(okResult.Value);
    }
    
    [Fact]
    public void Authenticate_Returns_Unauthorized_If_No_Credentials_Provided()
    {
        var requestBody = new AuthenticationController.AuthenticationRequestBody(null, null);

        var result = _authenticationController.Authenticate(requestBody);
        
        Assert.IsType<UnauthorizedResult>(result.Result);
    }
}