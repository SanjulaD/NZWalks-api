using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories.Auth;

public interface ITokenRepository
{
    string CreateJwtToken(IdentityUser user, List<string> roles);
}