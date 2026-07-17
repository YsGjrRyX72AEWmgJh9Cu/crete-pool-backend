using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the persistence mapping for <see cref="Participation"/>.
/// </summary>
public sealed class ParticipationConfiguration
    : IEntityTypeConfiguration<Participation>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Participation> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        ConfigureTable(builder);
        ConfigureKey(builder);
        ConfigureProperties(builder);
    }

    private static void ConfigureTable(EntityTypeBuilder<Participation> builder)
    {
        builder.ToTable("Participations");
    }

    private static void ConfigureKey(EntityTypeBuilder<Participation> builder)
    {
        builder.HasKey(participation => participation.Id);

        builder.Property(participation => participation.Id)
            .HasConversion(new StronglyTypedIdConverter<ParticipationId>())
            .ValueGeneratedNever();
    }

    private static void ConfigureProperties(EntityTypeBuilder<Participation> builder)
    {
        builder.Property(participation => participation.PlayerId)
            .HasConversion(new StronglyTypedIdConverter<PlayerId>())
            .IsRequired();

        builder.Property(participation => participation.TournamentId)
            .HasConversion(new StronglyTypedIdConverter<TournamentId>())
            .IsRequired();

        builder.Property(participation => participation.RegistrationDate)
            .IsRequired();
    }
}