using System.Data;

namespace Infrastructure.Helpers;

internal class UnitOfWorkSession
{
    private readonly IDbTransaction _dbTransaction;
    public UnitOfWorkSession(IDbConnection dbTransaction)
    {
        _dbTransaction = dbTransaction.BeginTransaction();
    }
    public void Commit()
    {
        _dbTransaction.Commit();
    }

    public void Rollback()
    {
        _dbTransaction.Rollback();
    }

    public IDbTransaction GetDbConnection()
    {
        return _dbTransaction;
    }
}