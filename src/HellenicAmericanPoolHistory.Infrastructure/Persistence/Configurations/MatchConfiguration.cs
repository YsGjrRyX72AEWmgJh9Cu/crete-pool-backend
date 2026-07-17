using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the persistence mapping for <see cref="Match"/>.
/// </summary>
public sealed class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        ConfigureTable(builder);
        ConfigureKey(builder);
        ConfigureProperties(builder);
    }

    private static void ConfigureTable(EntityTypeBuilder<Match> builder)
    {
        builder.ToTable("Matches");
    }

    private static void ConfigureKey(EntityTypeBuilder<Match> builder)
    {
        builder.HasKey(match => match.Id);

        builder.Property(match => match.Id)
            .HasConversion(new StronglyTypedIdConverter<MatchId>())
            .ValueGeneratedNever();
    }

    private static void ConfigureProperties(EntityTypeBuilder<Match> builder)
    {
        builder.Property(match => match.Participant1Id)
            .HasConversion(new StronglyTypedIdConverter<ParticipationId>())
            .IsRequired();

        builder.Property(match => match.Participant2Id)
            .HasConversion(new StronglyTypedIdConverter<ParticipationId>())
            .IsRequired();

        builder.Property(match => match.WinnerParticipationId)
            .HasConversion(new StronglyTypedIdConverter<ParticipationId>())
            .IsRequired();

        builder.Property(match => match.Participant1Score)
            .IsRequired();

        builder.Property(match => match.Participant2Score)
            .IsRequired();
    }
}