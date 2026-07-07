using Estapar.Domain.Entities;
using Estapar.Domain.Enums.Traffic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estapar.Infraestructure.Data.EntityTypeConfigurations;

/// <summary>
/// Entity Framework Core configuration for the <see cref="TrafficEntity"/> entity.
/// </summary>
/// <remarks>
/// This configuration class defines the database schema, constraints, relationships, and property mappings
/// for the Traffic entity, ensuring proper persistence and data integrity within the database.
/// </remarks>
public class TrafficEntityConfiguration : IEntityTypeConfiguration<TrafficEntity>
{
    /// <summary>
    /// Configures the entity mapping for <see cref="TrafficEntity"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<TrafficEntity> builder)
    {
        builder.ToTable("Traffics");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(t => t.LicensePlate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(t => t.Date)
            .IsRequired();

        builder.Property(t => t.LaneId)
            .IsRequired();

        builder.Property(t => t.Error)
            .IsRequired()
            .HasConversion<int>()
            .HasDefaultValue(TrafficError.None);

        builder.Property(t => t.Action)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(t => t.Success)
            .IsRequired();

        builder.Property(t => t.Balance)
            .IsRequired()
            .HasColumnType("numeric(18,2)");

        builder.Property(t => t.Created)
            .IsRequired();

        builder.Property(t => t.Updated)
            .IsRequired(false);

        builder.HasOne(t => t.Lane)
            .WithMany()
            .HasForeignKey(t => t.LaneId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.LicensePlate);
        builder.HasIndex(t => t.LaneId);
        builder.HasIndex(t => t.Date);
        builder.HasIndex(t => t.Created);
    }
}
