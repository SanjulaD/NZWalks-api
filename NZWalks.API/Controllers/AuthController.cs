using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO.Auth;
using NZWalks.API.Repositories.Auth;

namespace NZWalks.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenRepository _tokenRepository;
    private readonly UserManager<IdentityUser> _userManager;

    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        _userManager = userManager;
        _tokenRepository = tokenRepository;
    }

    [HttpPost]
    [Route("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.Username,
            Email = registerRequestDto.Username
        };
        var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);

        if (identityResult.Succeeded)
            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                if (identityResult.Succeeded) return Ok("User was registered! Please Login");
            }

        return BadRequest("Something went wrong");
    }

    [HttpPost]
    [Route("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await _userManager.FindByEmailAsync(loginRequestDto.Username);
        if (user != null)
        {
            var checkPasswordResult = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (!checkPasswordResult) return BadRequest("Username or password incorrect");
            var roles = await _userManager.GetRolesAsync(user);
            {
                var jwtToken = _tokenRepository.CreateJwtToken(user, roles.ToList());

                var response = new LoginResponseDto
                {
                    JwtToken = jwtToken
                };

                return Ok(response);
            }
        }

        return BadRequest("Username or password incorrect");
    }
}