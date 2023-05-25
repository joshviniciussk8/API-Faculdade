using FiapApi.Models;
using FiapApi.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Globalization;

namespace FiapApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TurmaController : ControllerBase
{

    private readonly ITurmaRepository _repository;
    private readonly IAlunosTurmaRepository _alunosTurmaRepository;
    public TurmaController(ITurmaRepository repository, IAlunosTurmaRepository alunosTurmaRepository)
    {
        _repository = repository;
        _alunosTurmaRepository = alunosTurmaRepository; 
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var turma = await _repository.BuscaTurmasAsync();

        return turma.Any() ? Ok(turma) : NoContent();
    }
    [HttpGet("id")]
    public async Task<IActionResult> Get(int id)
    {
        var turma = await _repository.BuscaTurmaIDAsync(id);

        return turma != null 
            ? Ok(turma) 
            : NotFound("Turma não encontrado");
    }
    [HttpPost]
    public async Task<IActionResult> Post(TurmaRequest request)
    {
        
        if (string.IsNullOrEmpty(request.turma) || request.ano<=0 || request.curso_id<=0 )
        {
            return BadRequest("Informações inválidas");
        }
        string anoAtual = DateTime.Now.ToString("yyyy");
        if (request.ano<int.Parse(anoAtual)) return BadRequest("Ano inválido");
        //validação de usuário existente!
        var TurmaDuplicada = await _repository.BuscaTurmaDuplicadaAsync(request.turma);
        
        if (TurmaDuplicada != null) return BadRequest("Essa turma já existe!");
        
        var adicionado = await _repository.AdicionaTurmaAsync(request);
        return adicionado
            ? Ok("Turma adicionada com sucesso")
            : BadRequest("Erro ao adicionar Turma");
    }
    [HttpPut]
    public async Task<IActionResult> Put(TurmaRequest request, int id)
    {
        if (id <= 0) return BadRequest("Turma inválida");

        var turma = await _repository.BuscaTurmaIDAsync(id);

        if (turma == null) NotFound("Essa turma não existe na base de dados");
        

        //validação de Turma existente!

        if (!string.IsNullOrEmpty(request.turma))
        {
            var turmaDuplicada = await _repository.BuscaTurmaDuplicadaAsync(request.turma);
            if (turmaDuplicada != null) return BadRequest("Essa turma já existe!");
        }
        if (string.IsNullOrEmpty(request.turma)) request.turma = turma.turma;
        if (request.curso_id==0) request.curso_id = turma.curso_id;

        if (request.ano>0)
        {
            string anoAtual = DateTime.Now.ToString("yyyy");
            if (request.ano < int.Parse(anoAtual)) return BadRequest("Ano inválido");
        }
        if (request.ano == 0) request.ano = turma.ano;

        

        var atualizado = await _repository.AtualizarTurmaAsync(request, id);
        return atualizado
            ? Ok("Turma atualizada com sucesso!")
            : BadRequest("Erro ao atualizar Turma");

    }
    [HttpDelete("id")]
    public async Task<IActionResult> Delete(int turma_id)
    {
        if (turma_id <= 0) return BadRequest("Turma inválida");
        var turma = await _repository.BuscaTurmaIDAsync(turma_id);
        if (turma == null) return NotFound("Essa Turma não existe na base de dados");
        //apaga aluno_turma relacionado com essa turma
        var aluno_turma = await _alunosTurmaRepository.BuscaAlunoTurmaByTurma(turma_id);
        foreach (var aluno in aluno_turma) await _alunosTurmaRepository.DeletarAsync(turma_id, aluno.aluno_id);

        var deletado = await _repository.DeletarTurmaAsync(turma_id);
        return deletado
            ? Ok("Turma deletada com sucesso!")
            : BadRequest("Erro ao deletar Turma");
    }
}
