using Microsoft.AspNetCore.Identity;

namespace DndDungeons.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
