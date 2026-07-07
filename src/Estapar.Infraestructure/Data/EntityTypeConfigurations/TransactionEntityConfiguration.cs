using Estapar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estapar.Infraestructure.Data.EntityTypeConfigurations;

/// <summary>
/// Entity Framework Core configuration for the <see cref="TransactionEntity"/> entity.
/// </summary>
/// <remarks>
/// This configuration class defines the database schema, constraints, relationships, and property mappings
/// for the Transaction entity, ensuring proper persistence and data integrity within the database.
/// </remarks>
public class TransactionEntityConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    /// <summary>
    /// Configures the entity mapping for <see cref="TransactionEntity"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(t => t.EntryTrafficId)
            .IsRequired();

        builder.Property(t => t.ExitTrafficId)
            .IsRequired();

        builder.Property(t => t.Balance)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(t => t.StayDuration)
            .IsRequired()
            .HasColumnType("interval");

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.Updated)
            .IsRequired(false);

        builder.HasOne(t => t.EntryTraffic)
            .WithMany()
            .HasForeignKey(t => t.EntryTrafficId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ExitTraffic)
            .WithMany()
            .HasForeignKey(t => t.ExitTrafficId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.EntryTrafficId)
            .IsUnique();

        builder.HasIndex(t => t.ExitTrafficId)
            .IsUnique();

        builder.HasIndex(t => t.Created);
    }
}
