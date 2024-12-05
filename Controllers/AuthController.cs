using BasicSaasTenent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user == null || user.TenantId != loginRequest.TenantId)
        {
            return Unauthorized(new { Message = "Invalid credentials or tenant." });
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password, false, false);
        if (!result.Succeeded)
        {
            return Unauthorized(new { Message = "Invalid login attempt." });
        }

        var token = await GenerateJwtToken(user);
        return Ok(new
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        });
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromBody] RegisterModel registerModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }

        ApplicationUser user = new()
        {
            Email = registerModel.Email,
            UserName = registerModel.UserName,
            TenantId = registerModel.TenantId
        };

        var result = await _userManager.CreateAsync(user, registerModel.Password);

        if (result.Succeeded)
        {
            return Ok(new { Message = "User registered successfully!" });
        }
        else
        {
            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("TenantId", user.TenantId)
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}