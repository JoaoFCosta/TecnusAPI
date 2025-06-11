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
using System.Collections.Generic; // Para List
using Microsoft.EntityFrameworkCore; // Para ToListAsync no GetAllUsers

namespace TecnusAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UserManager<AppUsuario> _userManager;
        private readonly SignInManager<AppUsuario> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager; // <-- Adicionado!

        public UsuarioController(
            UserManager<AppUsuario> userManager,
            SignInManager<AppUsuario> signInManager,
            IConfiguration configuration,
            RoleManager<IdentityRole> roleManager // <-- Adicionado!
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _roleManager = roleManager; // <-- Adicionado!
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
                // Opcional: Atribuir uma role padrão para novos usuários (ex: "User")
                // await _userManager.AddToRoleAsync(user, "User");
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
                var token = await GerarToken(appUser); // <-- Chame GerarToken como Task assíncrona
                return Ok(new { Message = "Login bem-sucedido!", Token = token, Username = appUser.Nome_Usuario });
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

        // Modificado para ser assíncrono (Task<string>) e incluir roles
        private async Task<string> GerarToken(AppUsuario usuario)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.GivenName, usuario.Nome_Usuario ?? usuario.UserName) // Usa Nome_Usuario ou UserName
            };

            // Adiciona as roles do usuário como claims no token JWT
            var roles = await _userManager.GetRolesAsync(usuario);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpireMinutes"] ?? "60")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("all")]
        // Proteger este endpoint para que apenas admins possam listar todos os usuários
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserListDTO>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // <-- Obter as roles de cada usuário
                userList.Add(new UserListDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    NomeCompleto = user.Nome_Usuario,
                    Telefone = user.PhoneNumber,
                });
            }
            return Ok(userList);
        }

        // --- NOVO: Endpoint para Adicionar um Usuário a uma Role ---
        // Exemplo: POST api/Usuario/add-role
        // Body: { "userId": "ID_DO_USUARIO_GUID", "roleName": "Admin" }
        [HttpPost("add-role")]
        //[Authorize(Roles = "Admin")] // Apenas usuários com a role "Admin" podem adicionar roles
        public async Task<IActionResult> AddRoleToUser([FromBody] UserRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.UserNome);
            if (user == null)
            {
                return NotFound($"Usuário com ID '{model.UserNome}' não encontrado.");
            }

            // Verifica se a role existe. É melhor criar a role via seeding (Program.cs)
            // ou em um endpoint separado para criação de roles.
            if (!await _roleManager.RoleExistsAsync(model.RoleName))
            {
                return NotFound($"Role '{model.RoleName}' não encontrada. Crie-a primeiro.");
            }

            if (await _userManager.IsInRoleAsync(user, model.RoleName))
            {
                return BadRequest($"Usuário '{user.UserName}' já está na role '{model.RoleName}'.");
            }

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            if (result.Succeeded)
            {
                return Ok(new { Message = $"Usuário '{user.UserName}' adicionado à role '{model.RoleName}' com sucesso." });
            }

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }

        // --- NOVO: Endpoint para Remover um Usuário de uma Role ---
        // Exemplo: POST api/Usuario/remove-role
        // Body: { "userId": "ID_DO_USUARIO_GUID", "roleName": "Admin" }
        [HttpPost("remove-role")]
        [Authorize(Roles = "Admin")] // Apenas usuários com a role "Admin" podem remover roles
        public async Task<IActionResult> RemoveRoleFromUser([FromBody] UserRoleModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(model.UserNome);
            if (user == null)
            {
                return NotFound($"Usuário com ID '{model.UserNome}' não encontrado.");
            }

            if (!await _roleManager.RoleExistsAsync(model.RoleName))
            {
                return NotFound($"Role '{model.RoleName}' não encontrada.");
            }

            if (!await _userManager.IsInRoleAsync(user, model.RoleName))
            {
                return BadRequest($"Usuário '{user.UserName}' não está na role '{model.RoleName}'.");
            }

            var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);

            if (result.Succeeded)
            {
                return Ok(new { Message = $"Usuário '{user.UserName}' removido da role '{model.RoleName}' com sucesso." });
            }

            return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
        }


        // --- NOVO: Endpoint para Listar Roles de um Usuário Específico ---
        // Exemplo: GET api/Usuario/{userId}/roles
        [HttpGet("{userId}/roles")]
        [Authorize(Roles = "Admin")] // Apenas admins podem ver as roles de outros usuários
        public async Task<ActionResult<IEnumerable<string>>> GetUserRoles(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Usuário com ID '{userId}' não encontrado.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        // --- NOVO: Endpoint para listar todas as Roles disponíveis ---
        // Exemplo: GET api/Usuario/all-roles
        [HttpGet("all-roles")]
        //[Authorize(Roles = "Admin")] // Apenas admins podem listar as roles
        public async Task<ActionResult<IEnumerable<string>>> GetAllRoles()
        {
            var roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return Ok(roles);
        }
    }
}