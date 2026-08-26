using HellenicAmericanPoolHistory.Domain.Organization;
using HellenicAmericanPoolHistory.Domain.TournamentSeries;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the persistence mapping for <see cref="TournamentSeries"/>.
/// </summary>
public sealed class TournamentSeriesConfiguration
    : IEntityTypeConfiguration<TournamentSeries>
{
    /// <inheritdoc />
    public void Configure(
        EntityTypeBuilder<TournamentSeries> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        ConfigureTable(builder);
        ConfigureKey(builder);
        ConfigureProperties(builder);
        ConfigureRelationships(builder);
    }

    private static void ConfigureTable(
        EntityTypeBuilder<TournamentSeries> builder)
    {
        builder.ToTable("TournamentSeries");
    }

    private static void ConfigureKey(
        EntityTypeBuilder<TournamentSeries> builder)
    {
        builder.HasKey(series => series.Id);

        builder.Property(series => series.Id)
            .HasConversion(
                new StronglyTypedIdConverter<TournamentSeriesId>())
            .ValueGeneratedNever();
    }

    private static void ConfigureProperties(
        EntityTypeBuilder<TournamentSeries> builder)
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
        EntityTypeBuilder<TournamentSeries> builder)
    {
        builder.HasOne<Organization>()
            .WithMany()
            .HasForeignKey(series => series.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
