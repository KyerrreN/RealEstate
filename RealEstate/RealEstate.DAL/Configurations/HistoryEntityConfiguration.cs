using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.DAL.Entities;

namespace RealEstate.DAL.Configurations
{
    public class HistoryEntityConfiguration : IEntityTypeConfiguration<HistoryEntity>
    {
        public void Configure(EntityTypeBuilder<HistoryEntity> builder)
        {
            builder
                .HasOne(h => h.RealEstate)
                .WithMany()
                .HasForeignKey(h => h.RealEstateId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
