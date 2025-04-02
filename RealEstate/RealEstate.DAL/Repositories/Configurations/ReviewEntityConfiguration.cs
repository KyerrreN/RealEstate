using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.DAL.Entities;

namespace RealEstate.DAL.Repositories.Configurations
{
    public class ReviewEntityConfiguration : IEntityTypeConfiguration<ReviewEntity>
    {
        public void Configure(EntityTypeBuilder<ReviewEntity> builder)
        {
            builder.HasOne(r => r.Author)
                .WithMany(u => u.AuthoredReviews)
                .HasForeignKey(r => r.AuthorId);

            builder.HasOne(r => r.Recipient)
                .WithMany(u => u.ReceivedReviews)
                .HasForeignKey(r => r.RecipientId);
        }
    }
}
