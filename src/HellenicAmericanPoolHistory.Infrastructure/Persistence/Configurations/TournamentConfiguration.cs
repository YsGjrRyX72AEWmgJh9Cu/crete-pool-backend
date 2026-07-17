using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the persistence mapping for <see cref="Tournament"/>.
/// </summary>
public sealed class TournamentConfiguration : IEntityTypeConfiguration<Tournament>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Tournament> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        ConfigureTable(builder);
        ConfigureKey(builder);
        ConfigureProperties(builder);
        ConfigureValueObjects(builder);
    }

    private static void ConfigureTable(EntityTypeBuilder<Tournament> builder)
    {
        builder.ToTable("Tournaments");
    }

    private static void ConfigureKey(EntityTypeBuilder<Tournament> builder)
    {
        builder.HasKey(tournament => tournament.Id);

        builder.Property(tournament => tournament.Id)
            .HasConversion(new StronglyTypedIdConverter<TournamentId>())
            .ValueGeneratedNever();
    }

    private static void ConfigureProperties(EntityTypeBuilder<Tournament> builder)
    {
        builder.Property(tournament => tournament.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(tournament => tournament.StartDate)
            .IsRequired();

        builder.Property(tournament => tournament.EndDate)
            .IsRequired();
    }

    private static void ConfigureValueObjects(EntityTypeBuilder<Tournament> builder)
    {
        builder.Property(tournament => tournament.Country)
            .HasConversion(
                country => country.Value,
                value => new Country(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(tournament => tournament.Discipline)
            .HasConversion(
                discipline => discipline.Value,
                value => new Discipline(value))
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(tournament => tournament.Category)
            .HasConversion(
                category => category.Value,
                value => new Category(value))
            .HasMaxLength(100)
            .IsRequired();
    }
}