
<h1 align="center">
  <br>
  <a href="http://www.google.com" target="_blank"><img src="https://raw.githubusercontent.com/srzsantiago/sentinelauth/master/assets/img/logo.jpg" alt="SentinelAuth" width="200"></a>
  <br>
  SentinelAuth
  <br>
</h1>

<h4 align="center">A simple and lightweight authentication and authorization library for .NET applications that provides password hashing using <a href="https://github.com/P-H-C/phc-winner-argon2" target="_blank">Argon2</a> for authentication and <a href="https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet" target="_blank">JWT</a> token generation for authorization.</h4>

<p align="center">
  <a href="https://badge.fury.io/nu/Cesala.SentinelAuth" target="_blank">
    <img src="https://badge.fury.io/nu/Cesala.SentinelAuth.svg"
         alt="Nugetpackage">
  </a>
  <a href="https://saythanks.io/to/srzsantiago" target="_blank">
      <img src="https://img.shields.io/badge/SayThanks.io-%E2%98%BC-1EAEDB.svg">
  </a>
  <a href="https://paypal.me/srzsantiago" target="_blank">
    <img src="https://img.shields.io/badge/$-donate-ff69b4.svg?maxAge=2592000&amp;style=flat">
  </a>
</p>

<p align="center">
  <a href="#key-features">Key Features</a> •
  <a href="#installation">Installation</a> •
  <a href="#how-to-use">How To Use</a> •
  <a href="#credits">Credits</a> •
  <a href="#license">License</a>
</p>

## Key Features

* Password Hashing
  - Uses <a href="https://github.com/P-H-C/phc-winner-argon2" target="_blank">Argon2</a> hashing algorithme following best practices.
* Role-based access control (RBAC) with JWT roles. 

## Installation
To install the package, use the NuGet package manager or run the following command:
```bash
dotnet add package Cesala.SentinelAuth
```

## How To Use

### Register Dependency Injection
Before using the package, register the necessary services in your Startup.cs or Program.cs file.

#### PasswordManager
Register the `PasswordManager` with the default (recommended) hashing settings:
```code
// Register PasswordManager for authentication with default settings
services.AddScoped<IPasswordManager, PasswordManager>();
```
If you want to apply a custom hashing configuration, provide a `HashingConfig` instance:
```code
// Register PasswordManager with custom configuration
services.AddScoped<IPasswordManager>(provider =>
{
    var config = new HashingConfig
    {
        MemorySize = 65536,
        DegreeOfParallelism = 4,
        Iterations = 4,
        SaltSize = 16,
        HashSize = 32,
    };
    return new PasswordManager(config);
});
```

#### TokenManager
The TokenManager requires a JwtConfig instance. Below is an example that retrieves configuration settings from `appsettings.json`.

⚠ **Security Warning**: Storing sensitive data (such as secret keys) in appsettings.json is a security risk. In production, store these values in a secure vault or use environment variables.
```code
// Register TokenManager for authorization
services.AddScoped<ITokenManager>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var jwtSettings = configuration.GetSection("JwtSettings");

    var config = new JwtConfig
    {
        Secret = jwtSettings["Secret"]!,
        Issuer = jwtSettings["Issuer"]!,
        Audience = jwtSettings["Audience"]!,
        ExpiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"]!)
    };

    return new TokenManager(config);
});
```

### Using the package
💡 **Note**: Inject the interface via the class constructor where you want to use the manager.

#### PasswordManager Usage
The `PasswordManager` helps with password hashing and validation.

```code
// Create a new account and generate a password hash
public User RegisterUser(RegisterRequest request)
{
    var hashedPassword = sentinelPasswordManager.CreateNewPasswordHash(request.Password);
    var user = new User { Username = request.Username, Password = hashedPassword };

    /**  Logic to add user to your system **/

    return user;
}

// Validate a login attempt (compare provided password with stored hash)
public User? ValidateUser(string username, string password)
{
    var user = /**  Logic to check if user exist and can be accessed **/
    if (!sentinelPasswordManager.VerifyPassword(password, user.PasswordHash))
        return null;

    return user;
}
```

#### TokenManager Usage
The `TokenManager` generates JWT tokens containing user ID, role, and username as claims.
```code
// Generate a JWT token for a user
public string GenerateJwtToken(User user)
{
    var sentinelUser = new SentinelUser
    {
        Id = user.Id,
        Role = user.Role.ToString(),
        Username = user.Username
    };

    return sentinelTokenManager.GenerateJwtToken(sentinelUser);
}
```

## Credits

This software uses the following open source packages:

- [Konscious.Security.Cryptography](https://github.com/kmaragon/Konscious.Security.Cryptography)
- [IdentityModel Extensions for .NET](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet?tab=readme-ov-file)

## License

[MIT License](LICENSE)

---

> [https://santiagorodriguez.xyz/](https://santiagorodriguez.xyz/) &nbsp;&middot;&nbsp;
> GitHub [@srzsantiago](https://github.com/srzsantiago) &nbsp;&middot;&nbsp;

