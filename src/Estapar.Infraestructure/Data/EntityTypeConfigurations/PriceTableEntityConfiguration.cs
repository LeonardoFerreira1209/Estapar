using Estapar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estapar.Infraestructure.Data.EntityTypeConfigurations;

/// <summary>
/// Entity Framework Core configuration for the <see cref="PriceTableEntity"/> entity.
/// </summary>
/// <remarks>
/// This configuration class defines the database schema, constraints, relationships, and property mappings
/// for the PriceTable entity, ensuring proper persistence and data integrity within the database.
/// </remarks>
public class PriceTableEntityConfiguration : IEntityTypeConfiguration<PriceTableEntity>
{
    /// <summary>
    /// Configures the entity mapping for <see cref="PriceTableEntity"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<PriceTableEntity> builder)
    {
        builder.ToTable("PriceTables");

        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(pt => pt.ParkId)
            .IsRequired();

        builder.Property(pt => pt.HourlyRate)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(pt => pt.GracePeriodMinutes)
            .IsRequired()
            .HasDefaultValue(30);

        builder.Property(pt => pt.Created)
            .IsRequired();

        builder.Property(pt => pt.Updated)
            .IsRequired(false);

        builder.HasOne(pt => pt.Park)
            .WithOne(p => p.PriceTable)
            .HasForeignKey<PriceTableEntity>(pt => pt.ParkId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(pt => pt.ParkId)
            .IsUnique();

        builder.HasIndex(pt => pt.Created);
    }
}
