using NSubstitute;
using SentinelAuth.Authentication;
using SentinelAuth.Config;
using SentinelAuth.Interfaces;
using Shouldly;

namespace SentinelAuth.Test;

[TestClass]
public sealed class PasswordManagerTest
{
    private PasswordManager _sut = new();

    private const string _CORRECT_PASSWORD = "testPass";
    private const string _WRONG_PASSWORD = "wrongPass";
    private const string _HASH_PREFIX = "$SENHASH$V1.0";
    private const string _MODIFIED_HASH_PREFIX = "$MODIFIED$V9.9";

    private const int _DEFAULT_CONFIG_MEMORYSIZE = 65536;
    private const int _DEFAULT_CONFIG_DEGREEOFPARALLELISM = 4;
    private const int _DEFAULT_CONFIG_ITERATIONS = 4;
    private const int _DEFAULT_CONFIG_SALTSIZE = 16;
    private const int _DEFAULT_CONFIG_HASHSIZE = 32;

    [TestMethod]
    public void ShouldVerifyCorrectPasswordTest()
    {
        // Arrange
        var newPasswordHash = _sut.CreateNewPasswordHash(_CORRECT_PASSWORD);

        // Act
        var result = _sut.VerifyPassword(_CORRECT_PASSWORD, newPasswordHash);

        // Assert
        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldVerifyIncorrectPasswordTest()
    {
        // Arrange
        var newPasswordHash = _sut.CreateNewPasswordHash(_CORRECT_PASSWORD);

        // Act
        var result = _sut.VerifyPassword(_WRONG_PASSWORD, newPasswordHash);

        // Assert
        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldVerifyModifiedPasswordPrefixTest()
    {
        // Arrange
        var newPasswordHash = _sut.CreateNewPasswordHash(_CORRECT_PASSWORD).Replace(_HASH_PREFIX, _MODIFIED_HASH_PREFIX);

        // Act
        var result = _sut.VerifyPassword(_CORRECT_PASSWORD, newPasswordHash);

        // Assert
        result.ShouldBeFalse();
    }

    [TestMethod]
    public void ShouldAllowCustomHashingConfigTest()
    {
        // Arrange
        HashingConfig config = new()
        {
            MemorySize = 1024,
            DegreeOfParallelism = 1,
            Iterations = 2,
            SaltSize = 32,
            HashSize = 64,
        };
        _sut = new PasswordManager(config);

        // Arrange
        var newPasswordHash = _sut.CreateNewPasswordHash(_CORRECT_PASSWORD);

        // Act
        var result = _sut.VerifyPassword(_CORRECT_PASSWORD, newPasswordHash);

        // Assert
        result.ShouldBeTrue();
    }

    [TestMethod]
    public void ShouldAllowCustomHashingConfigWithMockTest()
    {
        // Arrange
        IArgonWrapper mockArgonWrapper = Substitute.For<IArgonWrapper>();
        HashingConfig config = new()
        {
            MemorySize = 1024,
            DegreeOfParallelism = 1,
            Iterations = 2,
            SaltSize = 32,
            HashSize = 64,
        };
        _sut = new PasswordManager(config, mockArgonWrapper);

        // Act
        _ = _sut.CreateNewPasswordHash(_CORRECT_PASSWORD);

        // Assert
        mockArgonWrapper.Received().HashPassword(Arg.Any<byte[]>(), Arg.Any<byte[]>(), config.MemorySize, config.DegreeOfParallelism, config.Iterations, config.HashSize);
        mockArgonWrapper.Received().CreateSalt(config.SaltSize);
    }

    [TestMethod]
    public void ShouldSetDefaultHashingConfigWithMockTest()
    {
        // Arrange
        IArgonWrapper mockArgonWrapper = Substitute.For<IArgonWrapper>();
        _sut = new PasswordManager(new HashingConfig(), mockArgonWrapper);

        // Act
        _ = _sut.CreateNewPasswordHash(_CORRECT_PASSWORD);

        // Assert
        mockArgonWrapper.Received().HashPassword(Arg.Any<byte[]>(), Arg.Any<byte[]>(), _DEFAULT_CONFIG_MEMORYSIZE, _DEFAULT_CONFIG_DEGREEOFPARALLELISM, _DEFAULT_CONFIG_ITERATIONS, _DEFAULT_CONFIG_HASHSIZE);
        mockArgonWrapper.Received().CreateSalt(_DEFAULT_CONFIG_SALTSIZE);
    }
}
