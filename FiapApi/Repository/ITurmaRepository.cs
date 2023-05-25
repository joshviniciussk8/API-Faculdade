using FiapApi.Models;
namespace FiapApi.Repository
{
    public interface ITurmaRepository
    {
        Task<IEnumerable<TurmaResponse>> BuscaTurmasAsync();
        Task<TurmaResponse> BuscaTurmaIDAsync(int id);
        Task<TurmaResponse> BuscaTurmaDuplicadaAsync(string turma);
        Task<bool> AdicionaTurmaAsync(TurmaRequest request);
        Task<bool> AtualizarTurmaAsync(TurmaRequest request, int id);
        Task<bool> DeletarTurmaAsync(int id);

    }
}
