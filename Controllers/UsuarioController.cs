using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TecnusAPI.Models;
using TecnusAPI.DTO;
using Microsoft.AspNetCore.Authorization;

namespace TecnusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<AppUsuario> _userManager;
        private readonly SignInManager<AppUsuario> _signInManager;

        public UsuarioController(
            UserManager<AppUsuario> userManager,
            SignInManager<AppUsuario> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                // IMPLEMENTAR AQUI A LÓGICA DE GERAÇÃO DE JWT
                // Exemplo:
                // var roles = await _userManager.GetRolesAsync(appUser);
                // var token = _jwtService.GenerateToken(appUser, roles);

                return Ok(new { Message = "Login bem-sucedido!", /* token = token */ });
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
    }
}