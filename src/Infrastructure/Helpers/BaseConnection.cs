using System.Data;
using Dapper;

namespace Infrastructure.Helpers;

internal class BaseConnection
{
    private readonly IDbConnection _dbConnection;
    private UnitOfWorkSession? _unitOfWorkSession;
    public BaseConnection(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    protected async Task<T?> SelectAsync<T>(string sql, object model, CancellationToken cancellationToken) => await QueryAsync<T>(sql, cancellationToken, model);

    protected async Task<T?> SelectAsync<T>(string sql, CancellationToken cancellationToken) => await QueryAsync<T>(sql, cancellationToken);

    protected async Task<IEnumerable<T>> SelectAllAsync<T>(string sql, object model, CancellationToken cancellationToken) => await _dbConnection.QueryAsync<T>(BuildCommand(sql, cancellationToken, model));

    protected async Task<T> InsertAsync<T>(string sql, object model, CancellationToken cancellationToken) => (await QueryAsync<T>(sql, cancellationToken, model))!;

    protected async Task InsertAsync(string sql, object model, CancellationToken cancellationToken) => await UpdateAsync(sql, model, cancellationToken);

    protected async Task UpdateAsync(string sql, object model, CancellationToken cancellationToken)
    {
        await _dbConnection.ExecuteAsync(BuildCommand(sql, cancellationToken, model));
        return;
    }

    protected UnitOfWorkSession Begin()
    {
        if (_dbConnection.State != ConnectionState.Open) _dbConnection.Open();

        _unitOfWorkSession = new UnitOfWorkSession(_dbConnection);

        return _unitOfWorkSession;
    }

    protected void Join(UnitOfWorkSession token)
    {
        _unitOfWorkSession = (UnitOfWorkSession)token;
    }

    private CommandDefinition BuildCommand(string sql, CancellationToken cancellationToken, object? model = default) => new CommandDefinition(sql, model, cancellationToken: cancellationToken);

    private async Task<T?> QueryAsync<T>(string sql, CancellationToken cancellationToken, object? model = default) => await _dbConnection.QueryFirstOrDefaultAsync<T>(BuildCommand(sql, cancellationToken, model));
}