using Microsoft.AspNetCore.Identity;

namespace TecnusAPI.Models
{
    public class AppUsuario : IdentityUser
    {
        public string? Nome_Usuario { get; set; }
        public string? Email_Usuario { get; set; }
        public string? Telefone_Usuario { get; set; }
        public string? Senha_Usuario { get; set; }
    }
}
