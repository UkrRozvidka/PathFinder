using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DAL.Context.Config
{
    public class HikeConfig : IEntityTypeConfiguration<Hike>
    {
        void IEntityTypeConfiguration<Hike>.Configure(EntityTypeBuilder<Hike> builder)
        {
            builder.Property(u => u.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.AdminId)
                .IsRequired();

            builder.OwnsOne(h => h.Start, nav =>
            {
                nav.Property(p => p.Lat).HasColumnName("StartLat");
                nav.Property(p => p.Lon).HasColumnName("StartLon");
            });

            builder.OwnsOne(h => h.End, nav =>
            {
                nav.Property(p => p.Lat).HasColumnName("EndLat");
                nav.Property(p => p.Lon).HasColumnName("EndLon");
            });

            builder.HasMany(h => h.Tracks)
            .WithOne(t => t.Hike)
            .HasForeignKey(t => t.HikeId);

            builder.HasMany(h => h.HikeMembers)
                .WithOne(hm => hm.Hike)
                .HasForeignKey(hm => hm.HikeId);
        }
    }
}