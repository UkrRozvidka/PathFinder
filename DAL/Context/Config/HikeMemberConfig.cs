using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DAL.Context.Config
{
    public class HikeMemberConfig : IEntityTypeConfiguration<HikeMember>
    {
        void IEntityTypeConfiguration<HikeMember>.Configure(EntityTypeBuilder<HikeMember> builder)
        {
            builder.Property(u => u.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(u => u.UserId)
                .IsRequired();

            builder.Property(u => u.HikeId)
                .IsRequired();

            builder.HasMany(hm => hm.Points)
                .WithOne(p => p.HikeMember)
                .HasForeignKey(p => p.HikeMemberId);

            builder.HasOne(hm => hm.User)
                .WithMany(u => u.Hikes)
                .HasForeignKey(hm => hm.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
