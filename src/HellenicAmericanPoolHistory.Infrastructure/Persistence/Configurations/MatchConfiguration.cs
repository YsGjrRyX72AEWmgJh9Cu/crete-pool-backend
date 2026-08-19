using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Tournament;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the persistence mapping for <see cref="Match"/>.
/// </summary>
public sealed class MatchConfiguration
    : IEntityTypeConfiguration<Match>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        ConfigureTable(builder);
        ConfigureKey(builder);
        ConfigureProperties(builder);
        ConfigureRelationships(builder);
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
        builder.Property(match => match.TournamentId)
            .HasConversion(new StronglyTypedIdConverter<TournamentId>())
            .IsRequired();

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

    private static void ConfigureRelationships(
        EntityTypeBuilder<Match> builder)
    {
        builder.HasOne(match => match.Tournament)
            .WithMany(tournament => tournament.Matches)
            .HasForeignKey(match => match.TournamentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(match => match.Participant1)
            .WithMany()
            .HasForeignKey(match => match.Participant1Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(match => match.Participant2)
            .WithMany()
            .HasForeignKey(match => match.Participant2Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(match => match.Winner)
            .WithMany()
            .HasForeignKey(match => match.WinnerParticipationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}