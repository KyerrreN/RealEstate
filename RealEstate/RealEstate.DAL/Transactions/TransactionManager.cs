using RealEstate.DAL.Repositories;

namespace RealEstate.DAL.Transactions
{
    public class TransactionManager(AppDbContext context) : ITransactionManager
    {
        public async Task<ITransaction> BeginTransactionAsync(CancellationToken ct)
        {
            var transaction = await context.Database.BeginTransactionAsync(ct);

            return new EFCoreTransaction(transaction);
        }
    }
}
