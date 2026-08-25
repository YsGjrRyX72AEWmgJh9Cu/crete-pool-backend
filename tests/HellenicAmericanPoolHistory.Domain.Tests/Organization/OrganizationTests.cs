using OrganizationEntity =
    HellenicAmericanPoolHistory.Domain.Organization.Organization;

namespace HellenicAmericanPoolHistory.Domain.Tests.Organization;

public sealed class OrganizationTests
{
    [Fact]
    public void Create_Should_Create_Organization()
    {
        // Arrange
        const string name = "Hellenic American Pool Association";

        // Act
        var organization = OrganizationEntity.Create(name);

        // Assert
        Assert.NotEqual(default, organization.Id);
        Assert.Equal(name, organization.Name);
    }

    [Fact]
    public void Create_Should_Trim_Name()
    {
        // Arrange
        const string name = "  Hellenic American Pool Association  ";

        // Act
        var organization = OrganizationEntity.Create(name);

        // Assert
        Assert.Equal(
            "Hellenic American Pool Association",
            organization.Name);
    }

    [Fact]
    public void Create_Should_Throw_When_Name_Is_Empty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            OrganizationEntity.Create("   "));
    }

    [Fact]
    public void Rename_Should_Update_Name()
    {
        // Arrange
        var organization = CreateOrganization();

        // Act
        organization.Rename("  Updated Organization  ");

        // Assert
        Assert.Equal(
            "Updated Organization",
            organization.Name);
    }

    [Fact]
    public void Rename_Should_Throw_When_Name_Is_Empty()
    {
        // Arrange
        var organization = CreateOrganization();

        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            organization.Rename("   "));
    }

    private static OrganizationEntity CreateOrganization()
        => OrganizationEntity.Create(
            "Test Organization");
}
