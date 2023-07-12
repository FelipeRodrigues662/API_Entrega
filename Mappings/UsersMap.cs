using API_Eventos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Eventos.Mappings
{
    public class UsersMap : IEntityTypeConfiguration<Users>
    {
        public void Configure(EntityTypeBuilder<Users> builder)
        {
            builder.ToTable("Users");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd().UseIdentityColumn();

            builder.Property(u => u.Name).IsRequired().HasColumnName("Name").HasColumnType("NVARCHAR").HasMaxLength(256);
            builder.Property(u => u.Email).IsRequired().HasColumnName("Email").HasColumnType("NVARCHAR").HasMaxLength(256);
            builder.Property(u => u.Password).IsRequired().HasColumnName("Password").HasColumnType("NVARCHAR").HasMaxLength(256);
            builder.Property(u => u.User_Type).IsRequired().HasColumnName("User_Type").HasColumnType("NVARCHAR").HasMaxLength(50);
        }
    }
}
