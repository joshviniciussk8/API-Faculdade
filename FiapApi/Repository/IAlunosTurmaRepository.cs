using FiapApi.Models;
namespace FiapApi.Repository
{
    public interface IAlunosTurmaRepository
    {
        Task<IEnumerable<AlunosTurmaResponse>> BuscaAlunosTurmaAsync();
        Task<IEnumerable<AlunosTurmaResponse>> BuscaAlunoTurmaByAluno(int aluno_id);
        Task<IEnumerable<AlunosTurmaResponse>> BuscaAlunoTurmaByTurma(int turma_id);
        Task<AlunosTurmaResponse> BuscaAlunoTurmaDuplicado(int turma_id, int aluno_id);                
        Task<bool> AdicionaAsync(AlunosTurmaRequest request);        
        Task<bool> DeletarAsync(int turma_id, int aluno_id);

    }
}
