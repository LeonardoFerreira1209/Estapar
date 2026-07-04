using Estapar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estapar.Infraestructure.Data.EntityTypeConfigurations;

/// <summary>
/// Entity Framework Core configuration for the <see cref="GarageEntity"/> entity.
/// </summary>
/// <remarks>
/// This configuration class defines the database schema, constraints, relationships, and property mappings
/// for the Garage entity, ensuring proper persistence and data integrity within the database.
/// </remarks>
public class GarageEntityConfiguration : IEntityTypeConfiguration<GarageEntity>
{
    /// <summary>
    /// Configures the entity mapping for <see cref="GarageEntity"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<GarageEntity> builder)
    {
        builder.ToTable("Garages");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(g => g.ParkId)
            .IsRequired();

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(g => g.Created)
            .IsRequired();

        builder.Property(g => g.Updated)
            .IsRequired(false);

        builder.HasOne(g => g.Park)
            .WithMany(p => p.Garages)
            .HasForeignKey(g => g.ParkId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(g => g.ParkId);
        builder.HasIndex(g => g.Created);
    }
}
