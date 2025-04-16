namespace RealEstate.DAL.Transactions
{
    public interface ITransactionManager
    {
        Task<ITransaction> BeginTransactionAsync(CancellationToken ct);
    }
}
