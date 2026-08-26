using HellenicAmericanPoolHistory.Domain.Organization;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TournamentSeriesEntity =
    HellenicAmericanPoolHistory.Domain.TournamentSeries.TournamentSeries;
using HellenicAmericanPoolHistory.Domain.TournamentSeries;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the persistence mapping for <see cref="TournamentSeriesEntity"/>.
/// </summary>
public sealed class TournamentSeriesConfiguration
    : IEntityTypeConfiguration<TournamentSeriesEntity>
{
    /// <inheritdoc />
    public void Configure(
        EntityTypeBuilder<TournamentSeriesEntity> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        ConfigureTable(builder);
        ConfigureKey(builder);
        ConfigureProperties(builder);
        ConfigureRelationships(builder);
    }

    private static void ConfigureTable(
        EntityTypeBuilder<TournamentSeriesEntity> builder)
    {
        builder.ToTable("TournamentSeries");
    }

    private static void ConfigureKey(
        EntityTypeBuilder<TournamentSeriesEntity> builder)
    {
        builder.HasKey(series => series.Id);

        builder.Property(series => series.Id)
            .HasConversion(
                new StronglyTypedIdConverter<TournamentSeriesId>())
            .ValueGeneratedNever();
    }

    private static void ConfigureProperties(
        EntityTypeBuilder<TournamentSeriesEntity> builder)
    {
        builder.Property(series => series.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(series => series.OrganizationId)
            .HasConversion(
                new StronglyTypedIdConverter<OrganizationId>())
            .IsRequired();
    }

    private static void ConfigureRelationships(
        EntityTypeBuilder<TournamentSeriesEntity> builder)
    {
        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(series => series.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
