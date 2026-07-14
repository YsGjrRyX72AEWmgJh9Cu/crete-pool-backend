using HellenicAmericanPoolHistory.Domain.Common.Abstractions;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.ValueObjects;

namespace HellenicAmericanPoolHistory.Domain.Entities;

/// <summary>
/// Represents a pool tournament.
/// </summary>
public sealed class Tournament : Entity<TournamentId>
{
    public Tournament(
        TournamentId id,
        string name,
        Country country,
        Discipline discipline,
        Category category,
        DateOnly startDate,
        DateOnly endDate)
        : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        if (endDate < startDate)
            throw new ArgumentException(
                "End date cannot be earlier than start date.");

        Name = name.Trim();
        Country = country;
        Discipline = discipline;
        Category = category;
        StartDate = startDate;
        EndDate = endDate;
    }

    public string Name { get; }

    public Country Country { get; }

    public Discipline Discipline { get; }

    public Category Category { get; }

    public DateOnly StartDate { get; }

    public DateOnly EndDate { get; }
}