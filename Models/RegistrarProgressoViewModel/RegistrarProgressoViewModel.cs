using System.ComponentModel.DataAnnotations;

namespace TecnusAPI.Models.ViewModel
{
    public class RegistrarProgressoViewModel
    {
        [Required(ErrorMessage = "ID do usuário é obrigatório")]
        public string UsuarioId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ID do vídeo inválido")]
        public int VideoId { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Tempo não pode ser negativo")]
        public int TempoAssistidoSegundos { get; set; }
    }
}
