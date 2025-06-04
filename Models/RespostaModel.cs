using TecnusAPI.Models;

namespace TecnusAPI.Models
{
    public class RespostaModel
    {
        public int Id_Resposta { get; set; }
        public string Texto { get; set; }
        public bool Correta { get; set; }

        public int PerguntaId { get; set; }
        public PerguntaModel Pergunta { get; set; }
    }
}
