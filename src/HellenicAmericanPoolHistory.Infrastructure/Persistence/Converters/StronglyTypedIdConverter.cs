using System.Linq.Expressions;
using HellenicAmericanPoolHistory.Domain.Common.Abstractions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Converters;

/// <summary>
/// Converts strongly typed identifiers to and from their underlying <see cref="Guid"/> value.
/// </summary>
/// <typeparam name="TId">The strongly typed identifier.</typeparam>
public sealed class StronglyTypedIdConverter<TId> : ValueConverter<TId, Guid>
    where TId : struct, IStronglyTypedId<Guid>
{
    private static readonly Func<Guid, TId> Factory = CreateFactory();

    /// <summary>
    /// Initializes a new instance of the <see cref="StronglyTypedIdConverter{TId}"/> class.
    /// </summary>
    public StronglyTypedIdConverter()
        : base(
            id => id.Value,
            value => Factory(value))
    {
    }

    private static Func<Guid, TId> CreateFactory()
    {
        var constructor = typeof(TId).GetConstructor(new[] { typeof(Guid) });

        if (constructor is null)
        {
            throw new InvalidOperationException(
                $"{typeof(TId).Name} must declare a public constructor accepting a Guid.");
        }

        var parameter = Expression.Parameter(typeof(Guid), "value");

        var body = Expression.New(constructor, parameter);

        return Expression
            .Lambda<Func<Guid, TId>>(body, parameter)
            .Compile();
    }
}