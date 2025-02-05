using System.Security.Claims;
using NSubstitute;
using SentinelAuth.Authentication;
using SentinelAuth.Authorization;
using SentinelAuth.Config;
using SentinelAuth.Interfaces;
using SentinelAuth.Models;
using Shouldly;

namespace SentinelAuth.Test;

[TestClass]
public sealed class TokenManagerTest
{
    private TokenManager _sut = new(new JwtConfig() { Secret = "YourSuperSecureJWTSigningKey123456789", Issuer = "test", Audience = "test", ExpiryMinutes = 10 });

    [TestMethod]
    public void ShouldThrowExceptionInGenerateJwtTokenIfUserIsNullTest()
    {
        // Act & Assert
        Should.Throw<ArgumentNullException>(() => _sut.GenerateJwtToken(null!));
    }

    [TestMethod]
    public void ShouldGenerateJwtTokenTest()
    {
        // Arrange
        var user = new SentinelUser() { Id = Guid.NewGuid(), Username = "testUser", Role = "Admin" };

        // Act
        var result = _sut.GenerateJwtToken(user);

        // Assert
        result.ShouldNotBeEmpty();
    }

    [TestMethod]
    public void ShouldAllowCustomJwtConfigWithMockTest()
    {
        // Arrange
        IJwtWrapper mockJwtWrapper = Substitute.For<IJwtWrapper>();
        mockJwtWrapper.CreateTokenDescriptor(Arg.Any<ClaimsIdentity>(), Arg.Any<JwtConfig>(), Arg.Any<byte[]>())
            .Returns(new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor());
        JwtConfig config = new()
        { 
            Secret = "YourCustomSuperSecureJWTSigningKey123456789", 
            Issuer = "Customtest", 
            Audience = "Customtest", 
            ExpiryMinutes = 20 
        };
        var user = new SentinelUser() { Id = Guid.NewGuid(), Username = "testUser", Role = "Admin" };
        _sut = new TokenManager(config, mockJwtWrapper);

        // Act
        _ = _sut.GenerateJwtToken(user);

        // Assert
        mockJwtWrapper.Received().CreateIdentityClaim(user);
        mockJwtWrapper.Received().CreateTokenDescriptor(Arg.Any<ClaimsIdentity>(), config, Arg.Any<byte[]>());
    }
}
