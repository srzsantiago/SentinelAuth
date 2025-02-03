using SentinelAuth.Config;
using Shouldly;

namespace SentinelAuth.Test;

[TestClass]
public sealed class PasswordManagerTest
{
    private readonly PasswordManager _sut = new(new HashingConfig());

    [TestMethod]
    public void ShouldVerifyCorrectPasswordTest()
    {
        // Arrange
        var newPasswordHash = _sut.CreateNewPassword("testPass");

        // Act
        var result = _sut.VerifyPassword("testPass", newPasswordHash);

        // Assert
        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldVerifyIncorrectPasswordTest()
    {
        // Arrange
        var newPasswordHash = _sut.CreateNewPassword("testPass");

        // Act
        var result = _sut.VerifyPassword("wrongPass", newPasswordHash);

        // Assert
        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldVerifyModifiedPasswordPrefixTest()
    {
        // Arrange
        var newPasswordHash = _sut.CreateNewPassword("testPass").Replace("$SENHASH$V1.0", "$MODIFIED$V9.9");

        // Act
        var result = _sut.VerifyPassword("testPass", newPasswordHash);

        // Assert
        result.ShouldBeFalse();
    }
}
