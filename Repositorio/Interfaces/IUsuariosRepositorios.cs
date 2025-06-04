using TecnusAPI.Models;

namespace TecnusAPI.Repositorio.Interfaces
{
    public interface IUsuariosRepositorio
    {
        Task<List<UsuarioModel>> BuscarTodosUsuarios();
        Task<UsuarioModel> BuscarPorId(int id);
        Task<UsuarioModel> Adicionar(UsuarioModel usuario);
        Task<UsuarioModel> BuscarPorCpf(string CPF_Usuario);
        Task<UsuarioModel> BuscarPorEmail(string Email_Usuario);
        Task<UsuarioModel> Atualizar(UsuarioModel usuario, int id);
        Task<bool> Apagar(int id);

        Task<UsuarioModel> BuscarPorEmailECpf(string email, string cpf);
        Task<UsuarioModel> Atualizar_para_redefinicao_senha(UsuarioModel usuario);

        Task<UsuarioModel> AlterarSenha(AlterarSenhaModel alterarSenhaModel);
    }
}
