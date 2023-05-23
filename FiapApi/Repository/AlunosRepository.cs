using Dapper;
using FiapApi.Models;
using System.Data.SqlClient;
using System.Resources;

namespace FiapApi.Repository;

public class AlunosRepository : IAlunosRepository
{
    private readonly IConfiguration _configuration;
    private readonly string connectionString;

    public AlunosRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("SqlConnection");
    }
    public async Task<IEnumerable<AlunosResponse>> BuscaAlunosAsync()
    {
        string sql = @"select a.id, a.nome, a.usuario, a.senha from aluno a";
        using (var con = new SqlConnection(connectionString) )
        {
            return await con.QueryAsync<AlunosResponse>(sql);
        }
    }
    public Task<AlunosResponse> BuscaAlunoAsync(int id)
    {
        throw new NotImplementedException();
    }
    public Task<bool> AdicionaAsync(AlunosRequest request)
    {
        throw new NotImplementedException();
    }
    public Task<bool> AtualizarAsync(AlunosRequest request, int id)
    {
        throw new NotImplementedException();
    }
    public Task<bool> DeletarAsync(int id)
    {
        throw new NotImplementedException();
    }
}

