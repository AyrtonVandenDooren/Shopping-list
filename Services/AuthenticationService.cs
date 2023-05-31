namespace Shops.ShopServices;

public record UserInfo(string Username, string Name, string City);
public record AuthenticationRequestBody(string Username, string Password);
public interface IAuthenticationService
{
    UserInfo? ValidateUser(string username, string password);
    string? Authenticate(AuthenticationRequestBody authenticationRequestBody);
}
public class AuthenticationService : IAuthenticationService
{
    private readonly AuthenticationSettings _authenticationSettings;
    public AuthenticationService(IOptions<AuthenticationSettings>
    authenticationSettings)
    {
        _authenticationSettings = authenticationSettings.Value;
    }
    public UserInfo? ValidateUser(string username, string password)
    {
        return new UserInfo("Ayrtonvm", "Ayrton Van den Dooren", "Denderleeuw");
    }
    public string? Authenticate(AuthenticationRequestBody
    authenticationRequestBody)
    {
        UserInfo? userInfo = ValidateUser(authenticationRequestBody.Username,
        authenticationRequestBody.Password);
        if (userInfo == null) return null;
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.SecretForKey));
        var credentials = new SigningCredentials(securityKey,
        SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
        _authenticationSettings.Issuer,
        _authenticationSettings.Audience,
        new[]
        {
            new Claim("username", userInfo.Username),
            new Claim("name", userInfo.Name),
            new Claim("city", userInfo.City)
        },
        expires: DateTime.Now.AddMinutes(120),
        signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}