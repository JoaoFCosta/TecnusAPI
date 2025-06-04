namespace TecnusAPI.Models
{
    public class PerguntaModel
    {
        public int Id_Pergunta { get; set; }
        public string Texto { get; set; }

        public int QuizId { get; set; }
        public QuizModel Quiz { get; set; }

        public ICollection<RespostaModel> Respostas { get; set; } // ← Adicione esta linha
    }
}
