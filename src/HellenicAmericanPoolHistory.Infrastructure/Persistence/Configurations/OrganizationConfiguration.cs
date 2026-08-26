using HellenicAmericanPoolHistory.Domain.Organization;
using HellenicAmericanPoolHistory.Infrastructure.Persistence.Converters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HellenicAmericanPoolHistory.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configures the persistence mapping for <see cref="Organization"/>.
/// </summary>
public sealed class OrganizationConfiguration
    : IEntityTypeConfiguration<Organization>
{
    /// <inheritdoc />
    public void Configure(
        EntityTypeBuilder<Organization> builder)
    {
        ArgumentNullException.ThrowIfNull(builder);

        ConfigureTable(builder);
        ConfigureKey(builder);
        ConfigureProperties(builder);
    }

    private static void ConfigureTable(
        EntityTypeBuilder<Organization> builder)
    {
        builder.ToTable("Organizations");
    }

    private static void ConfigureKey(
        EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(organization => organization.Id);

        builder.Property(organization => organization.Id)
            .HasConversion(
                new StronglyTypedIdConverter<OrganizationId>())
            .ValueGeneratedNever();
    }

    private static void ConfigureProperties(
        EntityTypeBuilder<Organization> builder)
    {
        builder.Property(organization => organization.Name)
            .IsRequired()
            .HasMaxLength(200);
    }
}
