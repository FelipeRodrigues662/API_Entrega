using API_Eventos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Eventos.Mappings
{
    public class CategoriesMap : IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> builder)
        {
            builder.ToTable("Categories");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd().UseIdentityColumn();

            builder.Property(u => u.Name).IsRequired().HasColumnName("Name").HasColumnType("NVARCHAR").HasMaxLength(256);
        }
    }
}
