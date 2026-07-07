using Estapar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Estapar.Infraestructure.Data;

/// <summary>
/// Represents the database context for the Estapar system, providing access to and configuration of the
/// underlying data model.
/// </summary>
/// <remarks>This class extends <see cref="DbContext"/> and is designed to manage the database interactions for
/// the Estapar system. It provides an entry point for querying and saving Agrisk data, as well as configuring
/// the entity framework model through dedicated EntityTypeConfiguration classes.</remarks>
/// <param name="options">The options to be used by the <see cref="DbContext"/>.</param>
public class EstaparContext(
    DbContextOptions<EstaparContext> options
    ) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the DbSet for <see cref="ParkEntity"/>.
    /// </summary>
    public DbSet<ParkEntity> Parks { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="GarageEntity"/>.
    /// </summary>
    public DbSet<GarageEntity> Garages { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="LaneEntity"/>.
    /// </summary>
    public DbSet<LaneEntity> Lanes { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="PriceTableEntity"/>.
    /// </summary>
    public DbSet<PriceTableEntity> PriceTables { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="TrafficEntity"/>.
    /// </summary>
    public DbSet<TrafficEntity> Traffics { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="TransactionEntity"/>.
    /// </summary>
    public DbSet<TransactionEntity> Transactions { get; set; }

    /// <summary>
    /// Gets or sets the DbSet for <see cref="ParkedVehicleEntity"/>.
    /// </summary>
    public DbSet<ParkedVehicleEntity> ParkedVehicles { get; set; }

    /// <summary>
    /// Configures the model for the database context.
    /// </summary>
    /// <remarks>This method is called during the model creation process and applies all entity configurations
    /// from the current assembly. The configurations are defined in dedicated EntityTypeConfiguration classes
    /// to maintain separation of concerns and improve maintainability.</remarks>
    /// <param name="modelBuilder">The <see cref="ModelBuilder"/> used to configure the entity framework model.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(EstaparContext).Assembly
        );
    }
}