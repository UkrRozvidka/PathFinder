using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace DAL.Context.Config
{
    public class TrackConfig : IEntityTypeConfiguration<Track>
    {
        void IEntityTypeConfiguration<Track>.Configure(EntityTypeBuilder<Track> builder)
        {
            builder.Property(u => u.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(t => t.FileName)
            .IsRequired()
            .HasMaxLength(255);

            builder.Property(t => t.GpxFile)
                .IsRequired();
        }
    }
}