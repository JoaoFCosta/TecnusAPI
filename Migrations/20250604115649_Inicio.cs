using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TecnusAPI.Migrations
{
    /// <inheritdoc />
    public partial class Inicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tbl_Curso",
                columns: table => new
                {
                    Id_Curso = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_Curso = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descricao_Curso = table.Column<float>(type: "real", maxLength: 800, nullable: false),
                    Duracao_Curso = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Professor_Curso = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Curso", x => x.Id_Curso);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Feedback",
                columns: table => new
                {
                    Id_Feedback = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_Feedback = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Curso_Feedback = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Aula_Feedback = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Assunto_Feedback = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Feedback", x => x.Id_Feedback);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Quiz",
                columns: table => new
                {
                    Id_Quiz = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo_Quizz = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Quiz", x => x.Id_Quiz);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Usuario",
                columns: table => new
                {
                    Id_Usuario = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome_Usuario = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email_Usuario = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Telefone_Usuario = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Endereco_Usuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Complemento_Usuario = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CPF_Usuario = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: false),
                    CEP_Usuario = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Senha_Usuario = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Usuario", x => x.Id_Usuario);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Video",
                columns: table => new
                {
                    Id_Video = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Url = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DuracaoSegundos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Video", x => x.Id_Video);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Pergunta",
                columns: table => new
                {
                    Id_Pergunta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Pergunta", x => x.Id_Pergunta);
                    table.ForeignKey(
                        name: "FK_Tbl_Pergunta_Tbl_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Tbl_Quiz",
                        principalColumn: "Id_Quiz",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisualizacoesVideos",
                columns: table => new
                {
                    Id_Visualizacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VideoId_Visualizacao = table.Column<int>(type: "int", nullable: false),
                    UsuarioId_Visualizacao = table.Column<string>(type: "varchar(36)", nullable: false),
                    TempoAssistidoSegundos_Visualizacao = table.Column<int>(type: "int", nullable: false),
                    Concluido = table.Column<bool>(type: "bit", nullable: false),
                    Ultima_Visuailizacao = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisualizacoesVideos", x => x.Id_Visualizacao);
                    table.ForeignKey(
                        name: "FK_VisualizacoesVideos_Tbl_Video_VideoId_Visualizacao",
                        column: x => x.VideoId_Visualizacao,
                        principalTable: "Tbl_Video",
                        principalColumn: "Id_Video",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tbl_Resposta",
                columns: table => new
                {
                    Id_Resposta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Texto = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Correta = table.Column<bool>(type: "bit", nullable: false),
                    PerguntaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tbl_Resposta", x => x.Id_Resposta);
                    table.ForeignKey(
                        name: "FK_Tbl_Resposta_Tbl_Pergunta_PerguntaId",
                        column: x => x.PerguntaId,
                        principalTable: "Tbl_Pergunta",
                        principalColumn: "Id_Pergunta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Pergunta_QuizId",
                table: "Tbl_Pergunta",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Tbl_Resposta_PerguntaId",
                table: "Tbl_Resposta",
                column: "PerguntaId");

            migrationBuilder.CreateIndex(
                name: "IX_Visualizacoes_UsuarioVideo",
                table: "VisualizacoesVideos",
                columns: new[] { "UsuarioId_Visualizacao", "VideoId_Visualizacao" });

            migrationBuilder.CreateIndex(
                name: "IX_VisualizacoesVideos_VideoId_Visualizacao",
                table: "VisualizacoesVideos",
                column: "VideoId_Visualizacao");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tbl_Curso");

            migrationBuilder.DropTable(
                name: "Tbl_Feedback");

            migrationBuilder.DropTable(
                name: "Tbl_Resposta");

            migrationBuilder.DropTable(
                name: "Tbl_Usuario");

            migrationBuilder.DropTable(
                name: "VisualizacoesVideos");

            migrationBuilder.DropTable(
                name: "Tbl_Pergunta");

            migrationBuilder.DropTable(
                name: "Tbl_Video");

            migrationBuilder.DropTable(
                name: "Tbl_Quiz");
        }
    }
}
