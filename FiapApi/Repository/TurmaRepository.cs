using Dapper;
using FiapApi.Models;
using System.Data.SqlClient;
using System.Resources;

namespace FiapApi.Repository;

public class TurmaRepository : ITurmaRepository
{
    private readonly IConfiguration _configuration;
    private readonly string connectionString;

    public TurmaRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        connectionString = _configuration.GetConnectionString("SqlConnection");
    }           
    
    
    
    //Interface implementada
    public async Task<IEnumerable<TurmaResponse>> BuscaTurmasAsync()
    {
        string sql = @"select t.id, t.curso_id, t.ano, t.turma from turma t";
        using var con = new SqlConnection(connectionString);
        return await con.QueryAsync<TurmaResponse>(sql);
    }   

    public async Task<bool> AdicionaTurmaAsync(TurmaRequest request)
    {
        string sql = @"insert into turma(curso_id,turma,ano)values(@curso_id,@turma,@ano)";
        using var con = new SqlConnection(connectionString);
        return await con.ExecuteAsync(sql, request) > 0;
    }

    public async Task<bool> AtualizarTurmaAsync(TurmaRequest request, int id)
    {
        string sql = @"update turma set curso_id=@curso_id,ano=@ano,turma=@turma where id = @Id ";
        var parametros = new DynamicParameters();
        parametros.Add("curso_id", request.curso_id);
        parametros.Add("ano", request.ano);
        parametros.Add("turma", request.turma);
        parametros.Add("Id", id);
        using var con = new SqlConnection(connectionString);
        return await con.ExecuteAsync(sql, parametros) > 0;
    }

    public async Task<bool> DeletarTurmaAsync(int id)
    {
        string sql = @"delete from turma where id = @Id ";
        var parametros = new DynamicParameters();

        using var con = new SqlConnection(connectionString);
        return await con.ExecuteAsync(sql, new { Id = id }) > 0;
    }

    public async Task<TurmaResponse> BuscaTurmaDuplicadaAsync(string turma)
    {
        string sql = @"select t.id, t.curso_id, t.ano, t.turma from turma t where turma = @Turma";
        using var con = new SqlConnection(connectionString);
        return await con.QueryFirstOrDefaultAsync<TurmaResponse>(sql, new { Turma = turma });
    }

    public async Task<TurmaResponse> BuscaTurmaIDAsync(int id)
    {
        string sql = @"select t.id, t.curso_id, t.ano, t.turma from turma t where id = @Id";
        using var con = new SqlConnection(connectionString);
        return await con.QueryFirstOrDefaultAsync<TurmaResponse>(sql, new { Id = id });
    }
}

