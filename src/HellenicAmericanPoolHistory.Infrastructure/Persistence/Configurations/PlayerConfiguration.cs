using HellenicAmericanPoolHistory.Domain.ValueObjects;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the persistence mapping for <see cref="Player"/>.
/// </summary>
public sealed class PlayerConfiguration : IEntityTypeConfiguration<Player>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Player> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        ConfigureTable(builder);
        ConfigureKey(builder);
        ConfigureProperties(builder);
        ConfigureCountry(builder);
    }

    private static void ConfigureTable(EntityTypeBuilder<Player> builder)
    {
        builder.ToTable("Players");
    }

    private static void ConfigureKey(EntityTypeBuilder<Player> builder)
    {
        builder.HasKey(player => player.Id);

        builder.Property(player => player.Id)
            .HasConversion(new StronglyTypedIdConverter<PlayerId>())
            .ValueGeneratedNever();
    }

    private static void ConfigureProperties(EntityTypeBuilder<Player> builder)
    {
        builder.Property(player => player.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(player => player.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(player => player.BirthDate);
    }

    private static void ConfigureCountry(EntityTypeBuilder<Player> builder)
    {
        builder.Property(player => player.CountryOfOrigin)
            .HasConversion(
                country => country.Value,
                value => new Country(value))
            .HasMaxLength(100)
            .IsRequired();
    }
}