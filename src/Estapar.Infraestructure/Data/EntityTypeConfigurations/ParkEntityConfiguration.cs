using Estapar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estapar.Infraestructure.Data.EntityTypeConfigurations;

/// <summary>
/// Entity Framework Core configuration for the <see cref="ParkEntity"/> entity.
/// </summary>
/// <remarks>
/// This configuration class defines the database schema, constraints, relationships, and property mappings
/// for the Park entity, ensuring proper persistence and data integrity within the database.
/// </remarks>
public class ParkEntityConfiguration : IEntityTypeConfiguration<ParkEntity>
{
    /// <summary>
    /// Configures the entity mapping for <see cref="ParkEntity"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<ParkEntity> builder)
    {
        builder.ToTable("Parks");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(p => p.Created)
            .IsRequired();

        builder.Property(p => p.Updated)
            .IsRequired(false);

        builder.HasMany(p => p.Garages)
            .WithOne(g => g.Park)
            .HasForeignKey(g => g.ParkId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Lanes)
            .WithOne(l => l.Park)
            .HasForeignKey(l => l.ParkId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.PriceTable)
            .WithOne(pt => pt.Park)
            .HasForeignKey<PriceTableEntity>(pt => pt.ParkId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => p.Name);
        builder.HasIndex(p => p.Created);
    }
}
