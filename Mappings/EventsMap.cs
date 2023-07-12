using API_Eventos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Eventos.Mappings
{
    public class EventsMap : IEntityTypeConfiguration<Events>
    {
        public void Configure(EntityTypeBuilder<Events> builder)
        {
            builder.ToTable("Events");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd().UseIdentityColumn();

            builder.Property(u => u.Title).IsRequired().HasColumnName("Title").HasColumnType("NVARCHAR").HasMaxLength(256);
            builder.Property(u => u.Description).IsRequired().HasColumnName("Description").HasColumnType("NVARCHAR").HasMaxLength(256);

            builder.Property(u => u.Date).IsRequired().HasColumnName("Date").HasColumnType("DATETIME");

            builder.Property(u => u.Location).IsRequired().HasColumnName("Location").HasColumnType("NVARCHAR").HasMaxLength(256);

            builder.Property(u => u.Cartegory_Id).IsRequired().HasColumnName("Cartergoy_Id").HasColumnType("INT");

            builder.Property(u => u.Cartegory_Id).IsRequired().HasColumnName("Cartergoy_Id").HasColumnType("MONEY");

            builder.Property(u => u.Images).IsRequired().HasColumnName("Images").HasColumnType("NVARCHAR").HasMaxLength(256);

        }
    }
}
