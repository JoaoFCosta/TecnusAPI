using TecnusAPI.Seguranca;

namespace TecnusAPI.Models
{
    public class UsuarioModel
    {
        public int Id_Usuario { get; set; }
        public string? Nome_Usuario { get; set; }
        public string? Email_Usuario { get; set; }
        public string? Telefone_Usuario { get; set; }
        public string? Endereco_Usuario { get; set; }
        public string? Complemento_Usuario { get; set; }
        public string? CPF_Usuario { get; set; }
        public string? CEP_Usuario { get; set; }
        public string? Senha_Usuario { get; set; }

        public bool SenhaValida(string senha_usuario)
        {
            return Senha_Usuario == senha_usuario.GerarHash();
        }

        public void SetSenhaHash()
        {
            Senha_Usuario = Senha_Usuario.GerarHash();
        }

        public void SetNovaSenha(string novaSenha)
        {
            Senha_Usuario = novaSenha.GerarHash();
        }

        public string GerarNovaSenha()
        {
            string novaSenha = Guid.NewGuid().ToString().Substring(0, 8);
            Senha_Usuario = novaSenha.GerarHash();
            return novaSenha;
        }
    }
}
