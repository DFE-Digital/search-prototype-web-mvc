using Dfe.Data.SearchPrototype.SearchForEstablishments;
using Dfe.Data.SearchPrototype.Web.Models;
using Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Models;

public class EstablishmentViewModelTests
{
    [Fact]
    public void AddressAsString_ReturnsFormattedString()
    {
        EstablishmentViewModel establishmentViewModel = new()
        {
            Urn = EstablishmentViewModelTestDouble.GetEstablishmentIdentifierFake(),
            Name = EstablishmentViewModelTestDouble.GetEstablishmentNameFake(),
            Address = new()
            {
                Street = "street",
                Locality = "locality",
                Address3 = "address3",
                Town = "town",
                Postcode = "postcode",
            }
        };
        var expected = "street, locality, address3, town, postcode";
        var result = establishmentViewModel.AddressAsString();

        Assert.Equal(expected, result);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, "fakeLocality", "", "fakeTown", "FakePostCode", "fakeLocality, fakeTown, FakePostCode")]
    [InlineData(null, "", null, "fakeTown", "FakePostCode", "fakeTown, FakePostCode")]
    [InlineData("fakeStreet", null, "", null, "FakePostCode", "fakeStreet, FakePostCode")]
    [InlineData("", null, null, null, null, "")]
    [InlineData(null, null, null, null, null, "")]
    public void AddressAsString_NullAddressValues_ReturnsFormattedString(
         string street, string locality, string address3, string town, string postcode, string expectedString)
    {
        // arrange
        EstablishmentViewModel establishmentViewModel = new()
        {
            Urn = EstablishmentViewModelTestDouble.GetEstablishmentIdentifierFake(),
            Name = EstablishmentViewModelTestDouble.GetEstablishmentNameFake(),
            Address = new()
            {
                Street = street,
                Locality = locality,
                Address3 = address3,
                Town = town,
                Postcode = postcode
            }
        };
        var expected = expectedString;
        // act
        var result = establishmentViewModel.AddressAsString();
        // assert
        Assert.Equal(expected, result);
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(StatusCode.Open, "Open")]
    [InlineData(StatusCode.Closed, "Closed")]
    [InlineData(StatusCode.Unknown, "Unknown")]
    public void EstablishmentStatusAsString_ReturnsOpen(StatusCode statusCode, string expected)
    {
        EstablishmentViewModel establishmentViewModel = new()
        {
            Urn = EstablishmentViewModelTestDouble.GetEstablishmentIdentifierFake(),
            Name = EstablishmentViewModelTestDouble.GetEstablishmentNameFake(),
            EstablishmentStatusCode = statusCode
        };
        var result = establishmentViewModel.EstablishmentStatusAsString;
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(true, true, true, "Primary, Secondary, 16 plus")]
    [InlineData(false, true, true, "Secondary, 16 plus")]
    [InlineData(true, false, false, "Primary")]
    [InlineData(false, true, false, "Secondary")]
    [InlineData(false, false, true, "16 plus")]
    [InlineData(false, false, false, "")]
    public void EducationPhaseAsString_ReturnsFormattedString(bool isPrimary, bool isSecondary, bool isPost16, string expected)
    {
        EstablishmentViewModel establishmentViewModel = new()
        {
            Urn = EstablishmentViewModelTestDouble.GetEstablishmentIdentifierFake(),
            Name = EstablishmentViewModelTestDouble.GetEstablishmentNameFake(),
            EducationPhase = new()
            {
                IsPrimary = isPrimary,
                IsSecondary = isSecondary,
                IsPost16 = isPost16
            }
        };

        var result = establishmentViewModel.EducationPhaseAsString;

        result.Should().Be(expected);
    }
}
