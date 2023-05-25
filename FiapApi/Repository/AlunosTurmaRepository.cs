using Dapper;
using FiapApi.Models;
using System.Data.SqlClient;
using System.Resources;

namespace FiapApi.Repository;

public class AlunosTurmaRepository : IAlunosTurmaRepository
{
    private readonly IConfiguration _configuration;
    private readonly string connectionString;

    public AlunosTurmaRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("SqlConnection");
    }   
    
    //interface gerada
    public async Task<bool> AdicionaAsync(AlunosTurmaRequest request)
    {
        string sql = @"insert into aluno_turma(aluno_id,turma_id)values(@aluno_id,@turma_id)";
        using var con = new SqlConnection(connectionString);        
        return await con.ExecuteAsync(sql, request) > 0;
        
    }    
    public async Task<bool> DeletarAsync(int turma_id, int aluno_id)
    {
        string sql = @"delete from aluno_turma where turma_id=@Turma_id and aluno_id=@Aluno_id ";
        var parametros = new DynamicParameters();        
        using var con = new SqlConnection(connectionString);
        return await con.ExecuteAsync(sql, new { Turma_id = turma_id, Aluno_id = aluno_id }) > 0;
    }

    public async Task<IEnumerable<AlunosTurmaResponse>> BuscaAlunosTurmaAsync()
    {
        string sql = @"select alt.aluno_id, alt.turma_id, a.nome as nome, a.usuario as usuario, t.turma, t.ano as Ano from aluno a 
                        inner join aluno_turma alt on alt.aluno_id=a.id 
                        inner join turma t on t.id = alt.turma_id";
        using var con = new SqlConnection(connectionString);
        return await con.QueryAsync<AlunosTurmaResponse>(sql);
    }

    public async Task<IEnumerable<AlunosTurmaResponse>> BuscaAlunoTurmaByAluno(int aluno_id)
    {
        string sql = @"select alt.aluno_id, alt.turma_id, a.nome as Nome, a.usuario as Usuario, t.turma, t.ano as Ano from aluno a 
                        inner join aluno_turma alt on alt.aluno_id=a.id 
                        inner join turma t on t.id = alt.turma_id
                        where a.id = @Aluno_id";
        using var con = new SqlConnection(connectionString);
        return await con.QueryAsync<AlunosTurmaResponse>(sql, new { Aluno_id = aluno_id });
    }

    public async Task<AlunosTurmaResponse> BuscaAlunoTurmaDuplicado(int turma_id, int aluno_id)
    {
        string sql = @"select alt.aluno_id, alt.turma_id, a.nome as Nome, a.usuario as Usuario, t.turma, t.ano as Ano from aluno a 
                        inner join aluno_turma alt on alt.aluno_id=a.id 
                        inner join turma t on t.id = alt.turma_id
                        where a.id = @Aluno_id and t.id = @Turma_id";
        using var con = new SqlConnection(connectionString);
        return await con.QueryFirstOrDefaultAsync<AlunosTurmaResponse>(sql, new { Turma_id = turma_id, Aluno_id = aluno_id });
    }

    public async Task<IEnumerable<AlunosTurmaResponse>> BuscaAlunoTurmaByTurma(int turma_id)
    {
        string sql = @"select * from aluno_turma where turma_id = @Turma_id";
        using var con = new SqlConnection(connectionString);
        return await con.QueryAsync<AlunosTurmaResponse>(sql, new { Turma_id = turma_id });
    }
}

