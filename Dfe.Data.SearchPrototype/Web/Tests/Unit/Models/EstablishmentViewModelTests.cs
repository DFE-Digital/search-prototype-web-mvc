﻿using Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles;
using FluentAssertions;
using Xunit;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Models;

public class EstablishmentViewModelTests
{
    [Fact]
    public void AddressAsString_ReturnsFormattedString()
    {
        Establishment establishmentViewModel = new()
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
        var result = establishmentViewModel.ReadableAddress;

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
         string? street, string? locality, string? address3, string? town, string? postcode, string? expectedString)
    {
        // arrange
        Establishment establishmentViewModel = new()
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
        var result = establishmentViewModel.ReadableAddress;
        // assert
        Assert.Equal(expected, result);
        result.Should().Be(expected);
    }
}
