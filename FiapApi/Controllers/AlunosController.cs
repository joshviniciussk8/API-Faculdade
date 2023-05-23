using FiapApi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FiapApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlunosController : ControllerBase
{
    private readonly IAlunosRepository _repository;

    public AlunosController(IAlunosRepository repository)
    {
        _repository = repository;
    }
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var alunos = await _repository.BuscaAlunosAsync();

        return alunos.Any() ? Ok(alunos) : NoContent();
    }
}
