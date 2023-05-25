using FiapApi.Models;
using FiapApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FiapApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlunosController : ControllerBase
{

    private readonly IAlunosRepository _repository;
    private readonly IAlunosTurmaRepository _alunosTurmaRepository;
    public AlunosController(IAlunosRepository repository, IAlunosTurmaRepository alunosTurmaRepository)
    {
        _repository = repository;
        _alunosTurmaRepository = alunosTurmaRepository;
    }
    [HttpGet]    
    public async Task<IActionResult> Get()
    {
        var alunos = await _repository.BuscaAlunosAsync();

        return alunos.Any() ? Ok(alunos) : NoContent();
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(int id)
    {
        var aluno = await _repository.BuscaAlunoAsync(id);

        return aluno != null 
            ? Ok(aluno) 
            : NotFound("Aluno não encontrado");
    }
    [HttpPost]
    [Route("Novo")]
    public async Task<IActionResult> Post(AlunosRequest request)
    {
        if (string.IsNullOrEmpty(request.nome) || string.IsNullOrEmpty(request.usuario) || string.IsNullOrEmpty(request.senha))
        {
            return BadRequest("Informações inválidas");
        }
        //validação de usuário existente!
        var usuarioDuplicado = await _repository.BuscaAlunoUsuarioAsync(request.usuario);
        
        if (usuarioDuplicado != null) return BadRequest("Esse usuário já existe!");
        ChecaForcaSenha verificaSenha = new ChecaForcaSenha();
        ChecaForcaSenha.ForcaDaSenha forca;
        forca = verificaSenha.GetForcaDaSenha(request.senha);
        if (forca.ToString() == "Inaceitavel") return BadRequest("Senha Muito Fraca!");
        CriarMD5 criarMD5 = new CriarMD5();
        request.senha = criarMD5.RetornarMD5(request.senha);
        var adicionado = await _repository.AdicionaAsync(request);
        return adicionado
            ? Ok("Aluno adicionado com sucesso")
            : BadRequest("Erro ao adicionar Aluno");
    }
    [HttpPut]
    [Route("Alterar")]
    public async Task<IActionResult> Put(AlunosRequest request, int id)
    {
        if (id <= 0) return BadRequest("Aluno inválido");

        var aluno = await _repository.BuscaAlunoAsync(id);        

        if (aluno == null) NotFound("Esse Aluno não existe na base de dados");

        if (string.IsNullOrEmpty(request.nome)) request.nome = aluno.nome;

        //validação de usuário existente!

        if (!string.IsNullOrEmpty(request.usuario))
        {
            var usuarioDuplicado = await _repository.BuscaAlunoUsuarioAsync(request.usuario);
            if (usuarioDuplicado != null) return BadRequest("Esse usuário já existe!");
        }
        if (string.IsNullOrEmpty(request.senha))
        {
            request.senha = await _repository.BuscaSenhaAluno(id);
        }
        else
        {
            ChecaForcaSenha verificaSenha = new ChecaForcaSenha();
            ChecaForcaSenha.ForcaDaSenha forca;
            forca = verificaSenha.GetForcaDaSenha(request.senha);
            if (forca.ToString() == "Inaceitavel") return BadRequest("Senha Muito Fraca!");

            CriarMD5 criarMD5 = new CriarMD5();
            request.senha = criarMD5.RetornarMD5(request.senha);
        }
        if (string.IsNullOrEmpty(request.usuario)) request.usuario = aluno.usuario; 
      
        var atualizado = await _repository.AtualizarAsync(request, id);
        return atualizado
            ? Ok("Aluno atualizado com sucesso")
            : BadRequest("Erro ao atualizar Aluno");

    }
    [HttpDelete("id")]
    //[Route("Deletar")]
    public async Task<IActionResult> Delete(int aluno_id)
    {
        
        if (aluno_id <= 0) return BadRequest("Aluno inválido");
        var aluno = await _repository.BuscaAlunoAsync(aluno_id);
        if (aluno == null) return NotFound("Esse Aluno não existe na base de dados");
        //apaga a ligação dos alunos com as turmas antes de excluir o aluno
        var aluno_turma = await _alunosTurmaRepository.BuscaAlunoTurmaByAluno(aluno_id);
        foreach (var turma in aluno_turma) await _alunosTurmaRepository.DeletarAsync(turma.turma_id, aluno_id);        
        var deletado = await _repository.DeletarAsync(aluno_id);
        return deletado
            ? Ok("Aluno deletado com sucesso")
            : BadRequest("Erro ao deletar Aluno");
    }
}
