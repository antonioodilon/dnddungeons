using DndDungeons.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NZWalks.API.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly IConfiguration _configuration;

        public TokenRepository(IConfiguration configurationVar)
        {
            this._configuration = configurationVar;
        }
        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            // Create claims
            List<Claim> listClaims = new List<Claim>();

            /*
             JSON web tokens (JWTs) claims are pieces of information asserted about a subject.
            For example, an ID token (which is always a JWT) can contain a claim called name that
            asserts that the name of the user authenticating is "John Doe"
            */
            Claim claimToAdd = new Claim(ClaimTypes.Email, user.Email);
            listClaims.Add(claimToAdd);

            for (int i = 0; i < roles.Count; ++i)
            {
                Claim claimRoleToAdd = new Claim(ClaimTypes.Role, roles[i]);
                listClaims.Add(claimRoleToAdd);
            }

            // Now we are communication with the appsettings.json file and getting the key from "Jwt"
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // In how many hours, minutes or hours would this token expire?
            //double expirationTime = 15;
            //DateTime expiration = DateTime.Now.AddMinutes(expirationTime);

            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                listClaims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
