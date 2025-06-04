using Microsoft.AspNetCore.Mvc;
using TecnusAPI.Models;
using TecnusAPI.Repositorio.Interfaces;
using TecnusAPI.Utils;

namespace Tecnus_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioControler : ControllerBase
    {
        private readonly IUsuariosRepositorio _usuarioRepositorio;
        private readonly IEmail _email;

        public UsuarioControler(IUsuariosRepositorio usuarioRepositorio, IEmail email)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _email = email;
        }


        [HttpGet("Mostrar Todos Usuaios")]
        public async Task<ActionResult<List<UsuarioModel>>> ListarTodas()
        {
            try
            {
                List<UsuarioModel> usuario = await _usuarioRepositorio.BuscarTodosUsuarios();

                if (usuario == null || !usuario.Any())
                {
                    return NotFound("Nenhum usuario encontrado.");
                }

                return Ok(usuario); // Retorna codigo 200
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // Retorna codigo 400
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message); // Retorna codigo 401
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor."); // Retorna codigo 500
            }
        }

        [HttpGet("Buscar Cliente por id")]
        public async Task<ActionResult<UsuarioModel>> BuscarPorId(int id)
        {
            try
            {
                UsuarioModel usuario = await _usuarioRepositorio.BuscarPorId(id);
                if (usuario == null)
                {
                    return NotFound("Nenhum Clinte encontrado");
                }
                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro interno no servidor."); // Retorna codigo 500
            }

        }

        [HttpPost("Cadastrar Usuario")]
        public async Task<ActionResult<UsuarioModel>> Cadastrar([FromBody] UsuarioModel usuarioModel)
        {
            try
            {
                // Validação do modelo recebido
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // 400 - Dados inválidos
                }

                // Verifica se o CPF é válido
                if (!CpfValidator.ValidarCpfCliente(usuarioModel.CPF_Usuario))
                {
                    return BadRequest("CPF inválido."); // 400 - CPF inválido
                }

                // Verifica se o ID já existe
                if (usuarioModel.Id_Usuario > 0) // Supondo que ID 0 ou negativo não é permitido
                {
                    UsuarioModel clientePorId = await _usuarioRepositorio.BuscarPorId(usuarioModel.Id_Usuario);
                    if (clientePorId != null)
                    {
                        return Conflict("Já existe um usuario com este ID."); // 409 - Conflito
                    }
                }

                // Verifica se o CPF já existe
                UsuarioModel clienteExistentePorCpf = await _usuarioRepositorio.BuscarPorCpf(usuarioModel.CPF_Usuario);
                if (clienteExistentePorCpf != null)
                {
                    return Conflict("Já existe um cliente com este CPF."); // 409 - Conflito
                }

                // Verifica se o e-mail já existe
                UsuarioModel clientePorEmail = await _usuarioRepositorio.BuscarPorEmail(usuarioModel.Email_Usuario);
                if (clientePorEmail != null)
                {
                    return Conflict("Já existe um cliente com este e-mail."); // 409 - Conflito
                }

                // Adiciona o cliente ao repositório
                UsuarioModel usuario = await _usuarioRepositorio.Adicionar(usuarioModel);

                // Retorna o cliente criado com status 201 (Created)
                return CreatedAtAction(nameof(BuscarPorId), new { id = usuario.Id_Usuario }, usuario);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 400 - Argumento inválido
            }
            catch (Exception ex)
            {

                // Retorna erro 500 - Internal Server Error
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }
        }




        [HttpPut("Atualizar Usuario")]
        public async Task<ActionResult<UsuarioModel>> Atualizar([FromBody] UsuarioModel usuarioModel, int id)
        {
            try
            {
                UsuarioModel usuarioExistente = await _usuarioRepositorio.BuscarPorId(id);
                if (usuarioExistente == null)
                {
                    return NotFound("Cliente não encontrado."); // 404
                }

                // Verifica se o CPF é válido
                if (!CpfValidator.ValidarCpfCliente(usuarioModel.CPF_Usuario))
                {
                    return BadRequest("CPF inválido."); // 400
                }

                // Verifica se o CPF já existe em outro cliente
                UsuarioModel usuarioComMesmoCpf = await _usuarioRepositorio.BuscarPorCpf(usuarioModel.CPF_Usuario);
                if (usuarioComMesmoCpf != null && usuarioComMesmoCpf.Id_Usuario != id)
                {
                    return Conflict("Já existe outro cliente com este CPF."); // 409
                }

                // Verifica se o e-mail já existe em outro cliente
                UsuarioModel usuarioComMesmoEmail = await _usuarioRepositorio.BuscarPorEmail(usuarioModel.Email_Usuario);
                if (usuarioComMesmoEmail != null && usuarioComMesmoEmail.Id_Usuario != id)
                {
                    return Conflict("Já existe outro cliente com este e-mail."); // 409
                }

                usuarioModel.Id_Usuario = id;
                UsuarioModel usuario = await _usuarioRepositorio.Atualizar(usuarioModel, id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message); // 400 - Argumento inválido
            }
            catch (Exception ex)
            {

                // Retorna erro 500 - Internal Server Error
                return StatusCode(500, "Ocorreu um erro interno no servidor.");
            }

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<UsuarioModel>> Apagar(int id)
        {
            bool apagado = await _usuarioRepositorio.Apagar(id);
            return Ok(apagado);
        }

        [HttpPost]
        [Route("enviar-link-redefinir-senha")]
        public async Task<ActionResult<UsuarioModel>> EnviarLinkParaRedefinirSenha(RedefinirSenhaModel redefinirSenhaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UsuarioModel usuario = await _usuarioRepositorio.BuscarPorEmailECpf(redefinirSenhaModel.Email, redefinirSenhaModel.Cpf);

                    if (usuario != null)
                    {
                        string novaSenha = usuario.GerarNovaSenha();
                        string mensagem = $"Sua nova Senha é: {novaSenha}";

                        bool emailEnviado = _email.Enviar(usuario.Email_Usuario, "Tecnus - Nova Senha", mensagem);

                        if (emailEnviado)
                        {
                            await _usuarioRepositorio.Atualizar_para_redefinicao_senha(usuario);
                            return Ok(novaSenha);
                        }
                        else
                        {
                            return BadRequest("Email não foi enviado.");
                        }
                    }
                    return BadRequest("Usuario não encontrado.");
                }

                var erros = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new { Mensagem = "Dados inválidos.", Erros = erros });
            }
            catch (Exception e)
            {
                return StatusCode(500, "Ocorreu um erro no servidor.");
            }
        }

        [HttpPost("redefinir senha")]
        public async Task<ActionResult<UsuarioModel>> Alterar(AlterarSenhaModel alterarSenhaModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await _usuarioRepositorio.AlterarSenha(alterarSenhaModel);
                    var erros = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    // Retorna um BadRequest (400) com os erros de validação
                    return BadRequest(new
                    {
                        Mensagem = "Erro ao alterar senha.",
                        Erros = erros
                    });
                }


                return Ok(new { Mensagem = "Senha alterada com sucesso" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocorreu um erro no servidor.");
            }

        }
    }
}

