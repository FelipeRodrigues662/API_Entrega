using API_Eventos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Eventos.Mappings
{
    public class ReviewsMap : IEntityTypeConfiguration<Reviews>
    {
        public void Configure(EntityTypeBuilder<Reviews> builder)
        {
            builder.ToTable("Reviews");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd().UseIdentityColumn();

            builder.Property(u => u.User_Id).IsRequired().HasColumnName("User_id").HasColumnType("INT");
            builder.Property(u => u.Event_Id).IsRequired().HasColumnName("Events_id").HasColumnType("INT");
            builder.Property(u => u.Score).IsRequired().HasColumnName("Score").HasColumnType("INT");

            builder.Property(u => u.Comment).IsRequired().HasColumnName("Comment").HasColumnType("NVARCHAR").HasMaxLength(256);
        }
    }
}
