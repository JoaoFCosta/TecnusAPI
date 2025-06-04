using TecnusAPI.Models;

namespace TecnusAPI.Models
{
    public class VisualizacaoVideoModel
    {
        public int Id_Visualizacao { get; set; }
        public int VideoId_Visualizacao { get; set; }
        public string? UsuarioId_Visualizacao { get; set; }
        public int TempoAssistidoSegundos_Visualizacao { get; set; }
        public bool Concluido { get; set; }
        public DateTime Ultima_Visuailizacao { get; set; }

        public decimal PorcentagemAssistida
        {
            get
            {
                if (Video.DuracaoSegundos > 0)
                    return (decimal)TempoAssistidoSegundos_Visualizacao / Video.DuracaoSegundos * 100;
                return 0;
            }

        }
        public virtual VideoModel Video { get; set; }

    }


}
