using Estapar.Domain.Entities;
using Estapar.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estapar.Infraestructure.Data.EntityTypeConfigurations;

/// <summary>
/// Entity Framework Core configuration for the <see cref="LaneEntity"/> entity.
/// </summary>
/// <remarks>
/// This configuration class defines the database schema, constraints, relationships, and property mappings
/// for the Lane entity, ensuring proper persistence and data integrity within the database.
/// </remarks>
public class LaneEntityConfiguration : IEntityTypeConfiguration<LaneEntity>
{
    /// <summary>
    /// Configures the entity mapping for <see cref="LaneEntity"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<LaneEntity> builder)
    {
        builder.ToTable("Lanes");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(l => l.ParkId)
            .IsRequired();

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.LaneType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(l => l.Status)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(l => l.Created)
            .IsRequired();

        builder.Property(l => l.Updated)
            .IsRequired(false);

        builder.HasOne(l => l.Park)
            .WithMany(p => p.Lanes)
            .HasForeignKey(l => l.ParkId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(l => l.ParkId);
        builder.HasIndex(l => l.Name);
        builder.HasIndex(l => l.Status);
        builder.HasIndex(l => l.Created);
    }
}
