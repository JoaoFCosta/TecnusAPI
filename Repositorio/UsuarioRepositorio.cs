using Microsoft.EntityFrameworkCore;
using TecnusAPI.Data;
using TecnusAPI.Models;
using TecnusAPI.Repositorio.Interfaces;

namespace TecnusAPI.Repositorio
{
    public class UsuarioRepositorio : IUsuariosRepositorio
    {
        private readonly TecnusDBContext _dbContext;

        public UsuarioRepositorio(TecnusDBContext tecnusdbcontext)
        {
            _dbContext = tecnusdbcontext;

        }
        public async Task<UsuarioModel> BuscarPorId(int id)
        {
            return await _dbContext.Tbl_Usuario.FirstOrDefaultAsync(x => x.Id_Usuario == id);
        }

        public async Task<List<UsuarioModel>> BuscarTodosUsuarios()
        {
            return await _dbContext.Tbl_Usuario.ToListAsync();
        }
        public async Task<UsuarioModel> Adicionar(UsuarioModel usuario)
        {
            await _dbContext.Tbl_Usuario.AddAsync(usuario);
            await _dbContext.SaveChangesAsync();

            return usuario;
        }
        public async Task<UsuarioModel> Atualizar(UsuarioModel usuario, int id)
        {
            UsuarioModel usuarioPorId = await BuscarPorId(id);

            if (usuarioPorId == null)
            {
                throw new Exception($"Usuario para o ID: {id} não foi encontrado no banco de dados.");
            }
            usuarioPorId.Nome_Usuario = usuario.Nome_Usuario;
            usuarioPorId.Email_Usuario = usuario.Email_Usuario;
            usuarioPorId.Telefone_Usuario = usuario.Telefone_Usuario;
            usuarioPorId.Endereco_Usuario = usuario.Endereco_Usuario;
            usuarioPorId.Complemento_Usuario = usuario.Complemento_Usuario;
            usuarioPorId.CPF_Usuario = usuario.CPF_Usuario;
            usuarioPorId.CEP_Usuario = usuario.CEP_Usuario;
            usuarioPorId.Senha_Usuario = usuario.Senha_Usuario;

            _dbContext.Tbl_Usuario.Update(usuarioPorId);
            await _dbContext.SaveChangesAsync();
            return usuarioPorId;
        }

        public async Task<UsuarioModel> Atualizar_para_redefinicao_senha(UsuarioModel usuario)
        {
            UsuarioModel usuarioModel = await BuscarPorId(usuario.Id_Usuario);

            if (usuarioModel == null)
            {
                throw new Exception($"Usuario Para o Id: {usuario.Id_Usuario} não foi encontrado.");
            }

            usuarioModel.Nome_Usuario = usuario.Nome_Usuario;
            usuarioModel.Email_Usuario = usuario.Email_Usuario;
            usuarioModel.Telefone_Usuario = usuario.Telefone_Usuario;
            usuarioModel.Endereco_Usuario = usuario.Endereco_Usuario;
            usuarioModel.Complemento_Usuario = usuario.Complemento_Usuario;
            usuarioModel.CPF_Usuario = usuario.CPF_Usuario;
            usuarioModel.CEP_Usuario = usuario.CEP_Usuario;
            usuarioModel.Senha_Usuario = usuario.Senha_Usuario;

            _dbContext.Tbl_Usuario.Update(usuario);
            await _dbContext.SaveChangesAsync();
            return usuario;
        }

        public async Task<bool> Apagar(int id)
        {
            UsuarioModel usuarioPorId = await BuscarPorId(id);

            if (usuarioPorId == null)
            {
                throw new Exception($"Usuario para o ID: {id} não foi encontrado no banco de dados.");

            }
            _dbContext.Tbl_Usuario.Remove(usuarioPorId);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UsuarioModel> BuscarPorCpf(string CPF_Usuario)
        {
            return await _dbContext.Tbl_Usuario.FirstOrDefaultAsync(x => x.CPF_Usuario == CPF_Usuario);

        }

        public async Task<UsuarioModel> BuscarPorEmail(string Email_Usuario)
        {
            return await _dbContext.Tbl_Usuario.FirstOrDefaultAsync(e => e.Email_Usuario == Email_Usuario);
        }

        public async Task<UsuarioModel> BuscarPorEmailECpf(string email, string cpf)
        {
            return await _dbContext.Tbl_Usuario.FirstOrDefaultAsync(x => x.Email_Usuario == email && x.CPF_Usuario == cpf);
        }

        public async Task<UsuarioModel> AlterarSenha(AlterarSenhaModel alterarSenhaModel)
        {
            UsuarioModel usuarioModel = await BuscarPorId(alterarSenhaModel.Id);

            if (usuarioModel == null) throw new Exception("Houve um erro na atualização da senha, usuario nao encontrado!");

            if (usuarioModel.SenhaValida(alterarSenhaModel.SenhaAtual)) throw new Exception("Senha Atual não confere!");

            if (usuarioModel.SenhaValida(alterarSenhaModel.NovaSenha)) throw new Exception("Nova senha deve ser diferente da senha atual!");

            usuarioModel.SetNovaSenha(alterarSenhaModel.NovaSenha);

            _dbContext.Tbl_Usuario.Update(usuarioModel);
            _dbContext.SaveChanges();
            return usuarioModel;

        }

    }
}
