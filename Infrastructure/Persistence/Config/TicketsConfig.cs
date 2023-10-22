using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Config
{
    public class TicketsConfig : IEntityTypeConfiguration<Tickets>
    {
        public void Configure(EntityTypeBuilder<Tickets> entityBuilder)
        {
            entityBuilder.HasKey(t => t.TicketId);

            entityBuilder.Property(t => t.Usuario)
                .HasColumnName("Usuario")
                .HasMaxLength(50)
                .IsRequired();

            entityBuilder.HasOne(t => t.Funcion)
                .WithMany(f => f.Tickets)
                .HasForeignKey(t => t.FuncionId)
                .IsRequired();
        }
    }
}
