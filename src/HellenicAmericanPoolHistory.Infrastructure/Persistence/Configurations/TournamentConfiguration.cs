using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Domain.Venue;
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
        ConfigureRelationships(builder);
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

        builder.Property(tournament => tournament.TournamentType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(tournament => tournament.TournamentStatus)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(tournament => tournament.BracketType)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(tournament => tournament.GameSet)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(tournament => tournament.StartDate)
            .IsRequired();

        builder.Property(tournament => tournament.EndDate)
            .IsRequired();

        builder.Property(tournament => tournament.VenueId)
            .HasConversion(new StronglyTypedIdConverter<VenueId>())
            .IsRequired();
    }

    private static void ConfigureRelationships(EntityTypeBuilder<Tournament> builder)
    {
        builder.HasOne<Venue>()
            .WithMany()
            .HasForeignKey(tournament => tournament.VenueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}