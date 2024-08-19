using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApi.Entities;
using WebApi.Models;
using WebApi.Token;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public TokenController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> CreateToken([FromBody] InputLoginRequest input)
        {
            if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Password))
                return Unauthorized();



            var result = await _signInManager.PasswordSignInAsync(input.Email, input.Password, false, lockoutOnFailure: false);

            if (result.Succeeded)
            {

                var token = new TokenJWTBuilder()
                .AddSecurityKey(JwtSecurityKey.Create("Secret_Key-12345678"))
                .AddSubject("Claudio Matheus")
                .AddIssuer("Teste.Securiry.Bearer")
                .AddAudience("Teste.Securiry.Bearer")
                .AddExpiry(5)
                .Builder();

                return Ok(token.value);

            }
            else
            {
                return Unauthorized();
            }

        }

    }
}