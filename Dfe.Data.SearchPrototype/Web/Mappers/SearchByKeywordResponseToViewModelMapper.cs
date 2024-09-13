using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Dfe.Data.SearchPrototype.Web.Models;

namespace Dfe.Data.SearchPrototype.Web.Mappers;

/// <summary>
/// Facilitates mapping from the received T:Dfe.Data.SearchPrototype.SearchForEstablishments.SearchByKeywordResponse
/// into the required T:Dfe.Data.SearchPrototype.Web.Models.SearchResultsViewModel object.
/// </summary>
public class SearchByKeywordResponseToViewModelMapper : IMapper<SearchByKeywordResponse, SearchResultsViewModel>
{
    /// <summary>
    /// The mapping input is the use-case search response T:Dfe.Data.SearchPrototype.SearchForEstablishments.SearchByKeywordResponse
    /// and if any results are contained within the response a new P:Dfe.Data.SearchPrototype.Web.Models.SearchResultsViewModel.SearchItems
    /// instance is created.
    /// </summary>
    /// <param name="input">
    /// A configured T:Dfe.Data.SearchPrototype.SearchForEstablishments.SearchByKeywordResponse instance
    /// </param>
    /// <returns>
    /// A configured T:Dfe.Data.SearchPrototype.Web.Models.SearchResultsViewModel instance
    /// </returns>
    public SearchResultsViewModel MapFrom(SearchByKeywordResponse input)
    {
        SearchResultsViewModel viewModel = new();

        if (input.EstablishmentResults != null)
        {
            viewModel.SearchItems = new();
            foreach (var establishment in input.EstablishmentResults.Establishments)
            {
                viewModel.SearchItems.Add(new EstablishmentViewModel
                {
                    Urn = establishment.Urn,
                    Name = establishment.Name,
                    Address = new AddressViewModel()
                    {
                        Street = establishment.Address.Street,
                        Locality = establishment.Address.Locality,
                        Town = establishment.Address.Town,
                        Address3 = establishment.Address.Address3,
                        Postcode = establishment.Address.Postcode
                    },
                    PhaseOfEducation = establishment.PhaseOfEducation,
                    EstablishmentType = establishment.EstablishmentType,
                    EstablishmentStatusName = establishment.EstablishmentStatusName
                });
            }
        }
        if (input.EstablishmentFacetResults != null)
        {
            viewModel.Facets = [];

            foreach (var facet in input.EstablishmentFacetResults.Facets)
            {
                var facetValues = facet.Results.Select(
                    result => new FacetValue(
                          Value: result.Value,
                          Count: result.Count,
                          IsSelected: false
                        ))
                    .ToList();

                viewModel.Facets.Add(
                    new Facet(Name: facet.Name, Values: facetValues));
            };
        }
        return viewModel;
    }
}
