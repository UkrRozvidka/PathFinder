using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DAL.Context.Config
{
    public class PointConfig : IEntityTypeConfiguration<Point>
    {
        void IEntityTypeConfiguration<Point>.Configure(EntityTypeBuilder<Point> builder)
        {
            builder.Property(u => u.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(p => p.Priority)
                .IsRequired();

            builder.OwnsOne(p => p.GeoPoint, nav =>
            {
                nav.Property(g => g.Lat).HasColumnName("Lat");
                nav.Property(g => g.Lon).HasColumnName("Lon");
            });

            builder.HasOne(p => p.HikeMember)
                .WithMany(hm => hm.Points)
                .HasForeignKey(p => p.HikeMemberId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}