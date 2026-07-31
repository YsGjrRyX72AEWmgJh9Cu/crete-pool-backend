using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the persistence mapping for <see cref="Venue"/>.
/// </summary>
public sealed class VenueConfiguration : IEntityTypeConfiguration<Venue>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Venue> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        ConfigureTable(builder);
        ConfigureKey(builder);
        ConfigureProperties(builder);
        ConfigureLocation(builder);
    }

    private static void ConfigureTable(EntityTypeBuilder<Venue> builder)
    {
        builder.ToTable("Venues");
    }

    private static void ConfigureKey(EntityTypeBuilder<Venue> builder)
    {
        builder.HasKey(venue => venue.Id);

        builder.Property(venue => venue.Id)
            .HasConversion(new StronglyTypedIdConverter<VenueId>())
            .ValueGeneratedNever();
    }

    private static void ConfigureProperties(EntityTypeBuilder<Venue> builder)
    {
        builder.Property(venue => venue.Name)
            .IsRequired()
            .HasMaxLength(200);
    }

    private static void ConfigureLocation(EntityTypeBuilder<Venue> builder)
    {
        builder.OwnsOne(
            venue => venue.Location,
            location =>
            {
                location.Property(l => l.Country)
                    .HasColumnName("Country")
                    .HasMaxLength(100)
                    .IsRequired();

                location.Property(l => l.City)
                    .HasColumnName("City")
                    .HasMaxLength(100)
                    .IsRequired();

                location.Property(l => l.Address)
                    .HasColumnName("Address")
                    .HasMaxLength(250);
            });
    }
}