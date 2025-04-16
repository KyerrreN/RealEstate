namespace RealEstate.DAL.Transactions
{
    public interface ITransaction : IAsyncDisposable
    {
        Task CommitAsync(CancellationToken ct);
        Task RollbackAsync(CancellationToken ct);
    }
}
