using HellenicAmericanPoolHistory.Domain.Common.Abstractions;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.Venue;
using HellenicAmericanPoolHistory.Domain.Entities;
using HellenicAmericanPoolHistory.Domain.TournamentSeries;

namespace HellenicAmericanPoolHistory.Domain.Tournament;

public sealed class Tournament : Entity<TournamentId>
{
    private Tournament(
        TournamentId id,
        string name,
        TournamentType tournamentType,
        TournamentStatus tournamentStatus,
        BracketType bracketType,
        GameSet gameSet,
        DateOnly startDate,
        DateOnly endDate,
        VenueId venueId,
        TournamentSeriesId? tournamentSeriesId)
        : base(id)
    {
        Rename(name);

        if (endDate < startDate)
            throw new ArgumentException(
                "End date cannot be before start date.");

        TournamentType = tournamentType;
        TournamentStatus = tournamentStatus;
        BracketType = bracketType;
        GameSet = gameSet;
        StartDate = startDate;
        EndDate = endDate;
        VenueId = venueId;
        TournamentSeriesId = tournamentSeriesId;
    }

    public string Name { get; private set; } = null!;

    public TournamentType TournamentType { get; private set; }

    public TournamentStatus TournamentStatus { get; private set; }

    public BracketType BracketType { get; private set; }

    public GameSet GameSet { get; private set; }

    public DateOnly StartDate { get; private set; }

    public DateOnly EndDate { get; private set; }

    public VenueId VenueId { get; private set; }

    public TournamentSeriesId? TournamentSeriesId { get; private set; }

    public static Tournament Create(TournamentData data)
        => new(
            TournamentId.New(),
            data.Name,
            data.TournamentType,
            TournamentStatus.Draft,
            data.BracketType,
            data.GameSet,
            data.StartDate,
            data.EndDate,
            data.VenueId,
            data.TournamentSeriesId);

    public void Rename(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        Name = name.Trim();
    }

    /// <summary>
    /// Updates the tournament while it is still in Draft status.
    /// </summary>
    /// <param name="data">The updated tournament data.</param>
    public void Edit(TournamentData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        EnsureStatus(TournamentStatus.Draft);

        Rename(data.Name);

        if (data.EndDate < data.StartDate)
        {
            throw new ArgumentException(
                "End date cannot be before start date.");
        }

        TournamentType = data.TournamentType;
        BracketType = data.BracketType;
        GameSet = data.GameSet;
        StartDate = data.StartDate;
        EndDate = data.EndDate;
        VenueId = data.VenueId;
    }

    public void Schedule()
    {
        EnsureStatus(TournamentStatus.Draft);

        TournamentStatus = TournamentStatus.Scheduled;
    }

    public void Start()
    {
        EnsureStatus(TournamentStatus.Scheduled);

        TournamentStatus = TournamentStatus.InProgress;
    }

    public void Complete()
    {
        EnsureStatus(TournamentStatus.InProgress);

        TournamentStatus = TournamentStatus.Completed;
    }

    public void Cancel()
    {
        if (TournamentStatus is not TournamentStatus.Draft
            && TournamentStatus is not TournamentStatus.Scheduled)
        {
            throw new InvalidOperationException(
                "Only tournaments in Draft or Scheduled status can be cancelled.");
        }

        TournamentStatus = TournamentStatus.Cancelled;
    }

    private void EnsureStatus(TournamentStatus expectedStatus)
    {
        if (TournamentStatus != expectedStatus)
        {
            throw new InvalidOperationException(
                $"Operation requires tournament status '{expectedStatus}', but current status is '{TournamentStatus}'.");
        }
    }

    /// <summary>
    /// Gets the tournament participations.
    /// </summary>
    public ICollection<Participation> Participations { get; } =
        new List<Participation>();

    /// <summary>
    /// Gets the tournament matches.
    /// </summary>
    public ICollection<Match> Matches { get; } =
        new List<Match>();
}
