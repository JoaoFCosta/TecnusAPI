using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TecnusAPI.DTO;
using TecnusAPI.Models;

namespace TecnusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<AppUsuario> _userManager;
        private readonly SignInManager<AppUsuario> _signInManager;
        private readonly IConfiguration _configuration;

        public UsuarioController(
            UserManager<AppUsuario> userManager,
            SignInManager<AppUsuario> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUsuario
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.Telefone,
                Nome_Usuario = model.NomeCompleto,
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Usuário registrado com sucesso!", UserId = user.Id });
            }

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var appUser = await _userManager.FindByEmailAsync(model.Email);
                var token = GerarToken(appUser);
                return Ok(new { Message = "Login bem-sucedido!", Token = token });
            }

            if (result.IsLockedOut)
            {
                return Unauthorized(new { Message = "Conta bloqueada. Tente novamente mais tarde." });
            }
            if (result.IsNotAllowed)
            {
                return Unauthorized(new { Message = "Login não permitido. Confirme seu e-mail ou conta." });
            }

            return Unauthorized(new { Message = "Credenciais inválidas." });
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Message = "Usuário não autenticado." });
            }

            var appUser = await _userManager.FindByIdAsync(userId);

            if (appUser == null)
            {
                return NotFound(new { Message = "Perfil de usuário não encontrado." });
            }

            var userProfile = new UserProfileDTO
            {
                Id = appUser.Id,
                Email = appUser.Email,
                NomeCompleto = appUser.Nome_Usuario,
                Telefone = appUser.PhoneNumber,
            };

            return Ok(userProfile);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok(new { Message = "Logout realizado com sucesso." });
        }

        private string GerarToken(AppUsuario usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, usuario.Id),
        new Claim(ClaimTypes.Name, usuario.UserName)
    };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"] ?? "60")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}