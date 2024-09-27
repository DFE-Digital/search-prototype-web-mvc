using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Web.Mappers
{
    /// <summary>
    /// Facilitates mapping from the received <see cref="EstablishmentResults"/>,
    /// into the required list of <see cref="Models.ViewModels.Establishment"/> instances.
    /// </summary>
    public sealed class EstablishmentResultsToEstablishmentsViewModelMapper : IMapper<EstablishmentResults?, List<Models.ViewModels.Establishment>?>
    {
        /// <summary>
        /// Provides the functionality to map from the <see cref="EstablishmentResults"/>
        /// input, to a configured list of <see cref="Models.ViewModels.Establishment"/> instances.
        /// </summary>
        /// <param name="input">
        /// The provisioned <see cref="EstablishmentResults"> received
        /// from the use-case search request.
        /// </param>
        /// <returns>
        /// The configured list of <see cref="Models.ViewModels.Establishment"/> instances expected.
        /// </returns>
        public List<Models.ViewModels.Establishment>? MapFrom(EstablishmentResults? input)
        {
            List<Models.ViewModels.Establishment>? searchItems = null;

            if (input != null)
            {
                searchItems = [];

                foreach (var establishment in input.Establishments)
                {
                    searchItems.Add(new Models.ViewModels.Establishment
                    {
                        Urn = establishment.Urn,
                        Name = establishment.Name,
                        Address = new Models.ViewModels.Address()
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

            return searchItems;
        }
    }
}
