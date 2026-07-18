using HellenicAmericanPoolHistory.Domain.Common.Abstractions;
using HellenicAmericanPoolHistory.Domain.Identifiers;
using HellenicAmericanPoolHistory.Domain.ValueObjects;

namespace HellenicAmericanPoolHistory.Domain.Entities;

/// <summary>
/// Represents a player in the Hellenic American Pool History system.
/// </summary>
public sealed class Player : Entity<PlayerId>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Player"/> class.
    /// </summary>
    /// <param name="id">The player identifier.</param>
    /// <param name="firstName">The player's first name.</param>
    /// <param name="lastName">The player's last name.</param>
    /// <param name="countryOfOrigin">The player's country of origin.</param>
    /// <param name="birthDate">The player's birth date.</param>
    public Player(
        PlayerId id,
        string firstName,
        string lastName,
        Country countryOfOrigin,
        DateOnly? birthDate = null)
        : base(id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentNullException.ThrowIfNull(countryOfOrigin);

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        CountryOfOrigin = countryOfOrigin;
        BirthDate = birthDate;
    }

    /// <summary>
    /// Gets the player's first name.
    /// </summary>
    public string FirstName { get; }

    /// <summary>
    /// Gets the player's last name.
    /// </summary>
    public string LastName { get; }

    /// <summary>
    /// Gets the player's full name.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Gets the player's country of origin.
    /// </summary>
    public Country CountryOfOrigin { get; }

    /// <summary>
    /// Gets the player's birth date.
    /// </summary>
    public DateOnly? BirthDate { get; }
}