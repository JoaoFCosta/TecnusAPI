namespace TecnusAPI.Models
{
    public class QuizModel
    {
        public int Id_Quiz { get; set; }
        public string? Titulo_Quizz { get; set; }
        public ICollection<PerguntaModel>? Perguntas { get; set; }
    }
}
