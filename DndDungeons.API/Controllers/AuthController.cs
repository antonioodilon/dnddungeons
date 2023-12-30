using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DndDungeons.API.Data;
using DndDungeons.API.Models.DTO;
using DndDungeons.API.Repositories;

namespace NZWalks.API.Controllers
{
    //https://localhost:1234/api/auth
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly NZWalksAuthDbContext _dbContext; // Maybe we need this, or not
        private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        { 
            this._userManager = userManager;
            this._tokenRepository = tokenRepository;
        }
        // POST: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO registerRequestDTO)
        {
            IdentityUser identityUserVar = new IdentityUser
            {
                UserName = registerRequestDTO.Username,
                Email = registerRequestDTO.Username,
            };

            IdentityResult identityResult = await _userManager.CreateAsync(identityUserVar, registerRequestDTO.Password);

            if (identityResult.Succeeded)
            {
                // Add roles to this User
                if (registerRequestDTO.Roles != null && registerRequestDTO.Roles.Any()) //.Any() sees if a sequence has any elements
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUserVar, registerRequestDTO.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("User registered successfully! Please log in.");
                    }
                }
            } else
            {
                bool hasDigit = false;
                bool hasLowercase = false;
                bool hasUppercase = false;
                string smallLenMessage = "The password's length is less than 8. Please choose a password that has at least 8 characters";
                string hasNoDigitsMsg = "The password you chose has no numbers. Please choose a password that has at least 1 number";
                string hasNoLowercaseMsg = "The password you chose has no lowercase letters. Please choose a password that has at least 1 lowercase letter";
                string hasNoUppercaseMsg = "The password you chose has no uppercase letters. Please choose a password that has at least 1 uppercase letter";
                //1) If the length is less than 8
                if (registerRequestDTO.Password.Length < 8)
                {
                    return BadRequest(smallLenMessage);
                }

                for (int i = 0; i < registerRequestDTO.Password.Length; ++i)
                {
                    //2) If there are no digits:
                    if ((registerRequestDTO.Password[i] >= 48) && (registerRequestDTO.Password[i] <= 57))
                    {
                        hasDigit = true;
                    }

                    if ((!hasDigit) && (i == (registerRequestDTO.Password.Length - 1)))
                    {
                        return BadRequest(hasNoDigitsMsg);
                    }

                    //3) If there are no lowercase letters:
                    if ((registerRequestDTO.Password[i] >= 97) && (registerRequestDTO.Password[i] <= 122))
                    {
                        hasLowercase = true;
                    }

                    if ((!hasLowercase) &&(i == (registerRequestDTO.Password.Length - 1)))
                    {
                        return BadRequest(hasNoLowercaseMsg);
                    }

                    // 4) If there are no uppercase letters:
                    if ((registerRequestDTO.Password[i] >= 65) && (registerRequestDTO.Password[i] <= 90))
                    {
                        hasUppercase = true;
                    }

                    if ((!hasUppercase) && (i == (registerRequestDTO.Password.Length - 1)))
                    {
                        return BadRequest(hasNoUppercaseMsg);
                    }
                }
            }

            return BadRequest("An unknown error happened while registering a user. Please try again.");
        }

        // POST: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            IdentityUser userToFind = await _userManager.FindByEmailAsync(loginRequestDTO.Username);

            if (userToFind != null)
            {
                bool successCheckPassword = await _userManager.CheckPasswordAsync(userToFind, loginRequestDTO.Password);

                if (successCheckPassword)
                {
                    // Get Roles for this user:
                    IList<string> listRoles = await _userManager.GetRolesAsync(userToFind);

                    if (listRoles != null)
                    {
                        // Create Token
                        string jwtToken = _tokenRepository.CreateJWTToken(userToFind, listRoles.ToList());
                        LoginResponseDTO response = new LoginResponseDTO
                        {
                            JwtToken = jwtToken,
                        };
                        // Wasn't working before because in appsettings.json, inside of ["Jwt":"Key"], the key wasn't long enough.
                        // See this: https://stackoverflow.com/questions/47279947/idx10603-the-algorithm-hs256-requires-the-securitykey-keysize-to-be-greater

                        return Ok(response);
                    }
                    //return Ok("Login successful");
                }
            }

            return BadRequest("Username or password is incorrect");
            //throw new NotImplementedException();
        }
    }
}
