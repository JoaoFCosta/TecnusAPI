namespace TecnusAPI.DTO
{
    public class RegisterRequestDTO
    {
        public string NomeCompleto { get; set; } // Exemplo de campo customizado
        public string Email { get; set; }
        public string Password { get; set; }
        public string Telefone { get; set; }     // Para o PhoneNumber do IdentityUser
    }
}
