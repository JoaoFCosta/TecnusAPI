using System.ComponentModel.DataAnnotations;

namespace TecnusAPI.Models
{
    public class RedefinirSenhaModel
    {
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O Email fornecido não é válido.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O campo CPF é obrigatório.")]
        [StringLength(16, ErrorMessage = "O CPF deve ter exatamente 16 caracteres.")]
        public string? Cpf { get; set; }
    }
}