using Estapar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Estapar.Infraestructure.Data.EntityTypeConfigurations;

/// <summary>
/// Entity Framework Core configuration for the <see cref="ParkedVehicleEntity"/> entity.
/// </summary>
/// <remarks>
/// This is a high-throughput control table optimized for frequent INSERT and DELETE operations.
/// Indexes are kept minimal and targeted to support the most critical queries:
/// license plate lookups (entry/exit validation) and garage occupancy counts.
/// </remarks>
public class ParkedVehicleEntityConfiguration : IEntityTypeConfiguration<ParkedVehicleEntity>
{
    /// <summary>
    /// Configures the entity mapping for <see cref="ParkedVehicleEntity"/>.
    /// </summary>
    /// <param name="builder">The builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<ParkedVehicleEntity> builder)
    {
        builder.ToTable("ParkedVehicles");

        builder.HasKey(pv => pv.Id);

        builder.Property(pv => pv.Id)
            .IsRequired()
            .ValueGeneratedNever();

        builder.Property(pv => pv.LicensePlate)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(pv => pv.EntryTrafficId)
            .IsRequired();

        builder.Property(pv => pv.GarageId)
            .IsRequired();

        builder.Property(pv => pv.Created)
            .IsRequired();

        builder.Property(pv => pv.Updated)
            .IsRequired(false);

        builder.HasOne(pv => pv.EntryTraffic)
            .WithMany()
            .HasForeignKey(pv => pv.EntryTrafficId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pv => pv.Garage)
            .WithMany(g => g.ParkedVehicles)
            .HasForeignKey(pv => pv.GarageId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique: a vehicle can only be registered once across the entire system at any time
        builder.HasIndex(pv => pv.LicensePlate)
            .IsUnique();

        // For fast occupancy queries and count per garage
        builder.HasIndex(pv => pv.GarageId);

        // One entry traffic can only generate one parked vehicle record
        builder.HasIndex(pv => pv.EntryTrafficId)
            .IsUnique();
    }
}
