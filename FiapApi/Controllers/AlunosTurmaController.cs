using FiapApi.Models;
using FiapApi.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;

namespace FiapApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlunosTurmaController : ControllerBase
{

    private readonly IAlunosTurmaRepository _repository;
    private readonly ITurmaRepository _repository_turma;
    private readonly IAlunosRepository _repository_Aluno;
    public AlunosTurmaController(IAlunosTurmaRepository repository, ITurmaRepository repository2, IAlunosRepository repository_Aluno)
    {
        _repository = repository;
        _repository_turma = repository2;        
        _repository_Aluno = repository_Aluno;   
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var Aluno_turma = await _repository.BuscaAlunosTurmaAsync();

        return Aluno_turma.Any() ? Ok(Aluno_turma) : NoContent();
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(int aluno_id)
    {
        var Aluno_turma = await _repository.BuscaAlunoTurmaByAluno(aluno_id);

        return Aluno_turma != null 
            ? Ok(Aluno_turma) 
            : NotFound("Aluno_Turma não encontrada");
    }
    [HttpGet("turma_id/aluno_id")]
    public async Task<IActionResult> GetDuplicado(int turma_id, int aluno_id)
    {
        var Aluno_turma = await _repository.BuscaAlunoTurmaDuplicado(turma_id,aluno_id);

        return Aluno_turma != null
            ? Ok(Aluno_turma)
            : NotFound("Aluno_Turma não encontrada");
    }
    [HttpPost]
    public async Task<IActionResult> Post(AlunosTurmaRequest request)
    {

        //validação de usuário existente!
        if (request.turma_id <= 0) return BadRequest("Turma inválida");
        var turma = await _repository_turma.BuscaTurmaIDAsync(request.turma_id);
        if (turma == null) return NotFound("Essa Turma não existe na base de dados");
        if (request.aluno_id <= 0) return BadRequest("Aluno inválio");
        var aluno = await _repository_Aluno.BuscaAlunoAsync(request.aluno_id);
        if (aluno == null) return NotFound("Esse Aluno não existe na base de dados");
        //validação de turma_aluno duplicada
        var duplicado = await _repository.BuscaAlunoTurmaDuplicado(request.turma_id,request.aluno_id);
        if (duplicado != null) return BadRequest("Esse aluno já consta nessa turma!");
        var adicionado = await _repository.AdicionaAsync(request);
        return adicionado
            ? Ok("Aluno adicionado a Turma com sucesso")
            : BadRequest("Erro ao adicionar Aluno a Turma");
    }
    
    [HttpDelete("id")]
    public async Task<IActionResult> Delete(int turma_id, int aluno_id)
    {
        if (turma_id <= 0) return BadRequest("Turma inválida");
        var turma = await _repository_turma.BuscaTurmaIDAsync(turma_id);
        if (turma == null) return NotFound("Essa Turma não existe na base de dados");
        if (aluno_id<=0)return BadRequest("Aluno inválio");
        var aluno = await _repository_Aluno.BuscaAlunoAsync(aluno_id);
        if (aluno == null) return NotFound("Esse Aluno não existe na base de dados");
        var deletado = await _repository.DeletarAsync(turma_id, aluno_id);
        return deletado
            ? Ok("Aluno removido da turma com sucesso!")
            : BadRequest("Erro ao remover aluno");
    }
}
