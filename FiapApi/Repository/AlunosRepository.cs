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
        string sql = @"select a.id, a.nome, a.usuario from aluno a";
        using var con = new SqlConnection(connectionString);
        return await con.QueryAsync<AlunosResponse>(sql);
        
    }
    public async Task<AlunosResponse> BuscaAlunoAsync(int id)
    {
        string sql = @"select a.id, a.nome, a.usuario, a.senha from aluno a where id = @Id";
        using var con = new SqlConnection(connectionString);        
        return await con.QueryFirstOrDefaultAsync<AlunosResponse>(sql, new {Id = id});
        
    }
    public async Task<string> BuscaSenhaAluno(int id)
    {
        string sql = @"select a.senha from aluno a where id = @Id";
        using var con = new SqlConnection(connectionString);
        return await con.QueryFirstOrDefaultAsync<string>(sql, new { Id = id });

    }
    public async Task<AlunosResponse> BuscaAlunoUsuarioAsync(string usuario)
    {
        string sql = @"select a.id, a.nome, a.usuario from aluno a where usuario = @Usuario";
        using var con = new SqlConnection(connectionString);
        return await con.QueryFirstOrDefaultAsync<AlunosResponse>(sql, new { Usuario = usuario });

    }
    public async Task<bool> AdicionaAsync(AlunosRequest request)
    {
        string sql = @"insert into aluno(nome,usuario,senha)values(@nome,@usuario,@senha)";
        using var con = new SqlConnection(connectionString);        
        return await con.ExecuteAsync(sql, request) > 0;
        
    }
    public async Task<bool> AtualizarAsync(AlunosRequest request, int id)
    {
        string sql = @"update aluno set nome=@nome,usuario=@usuario,senha=@senha where id = @Id ";
        var parametros = new DynamicParameters();
        parametros.Add("nome", request.nome);
        parametros.Add("usuario", request.usuario); 
        parametros.Add("senha", request.senha);
        parametros.Add("Id", id);
        using var con = new SqlConnection(connectionString);
        return await con.ExecuteAsync(sql, parametros) > 0;
    }
    public async Task<bool> DeletarAsync(int id)
    {
        string sql = @"delete from aluno where id = @Id ";
        var parametros = new DynamicParameters();
        
        using var con = new SqlConnection(connectionString);
        return await con.ExecuteAsync(sql, new {Id = id}) > 0;
    }
}

