using FiapApi.Models;
namespace FiapApi.Repository
{
    public interface IAlunosRepository
    {
        Task<IEnumerable<AlunosResponse>> BuscaAlunosAsync();
        Task<AlunosResponse> BuscaAlunoAsync(int id);
        Task<string> BuscaSenhaAluno(int id);
        Task<AlunosResponse> BuscaAlunoUsuarioAsync(string usuario);
        Task<bool> AdicionaAsync(AlunosRequest request);
        Task<bool> AtualizarAsync(AlunosRequest request, int id);
        Task<bool> DeletarAsync(int id);

    }
}
