using Microsoft.EntityFrameworkCore.Storage;

namespace RealEstate.DAL.Transactions
{
    public sealed class EFCoreTransaction(IDbContextTransaction transaction) : ITransaction
    {
        public async Task CommitAsync(CancellationToken ct)
        {
            await transaction.CommitAsync(ct);
        }

        public async ValueTask DisposeAsync()
        {
            await transaction.DisposeAsync();
        }

        public async Task RollbackAsync(CancellationToken ct)
        {
            await transaction.RollbackAsync(ct);
        }
    }
}
