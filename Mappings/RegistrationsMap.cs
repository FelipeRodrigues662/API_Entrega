using API_Eventos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace API_Eventos.Mappings
{
    public class RegistrationsMap : IEntityTypeConfiguration<Registrations>
    {
        public void Configure(EntityTypeBuilder<Registrations> builder)
        {
            builder.ToTable("Registrations");
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).ValueGeneratedOnAdd().UseIdentityColumn();

            builder.Property(u => u.User_Id).IsRequired().HasColumnName("User_id").HasColumnType("INT");
            builder.Property(u => u.Event_Id).IsRequired().HasColumnName("Events_id").HasColumnType("INT");

            builder.Property(u => u.Payment_Status).IsRequired().HasColumnName("Payment_Status").HasColumnType("BIT").HasDefaultValue(false);
        }
    }
}
