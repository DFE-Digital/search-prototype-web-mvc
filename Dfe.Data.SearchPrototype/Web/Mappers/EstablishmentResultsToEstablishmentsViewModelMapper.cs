using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Web.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EstablishmentResultsToEstablishmentsViewModelMapper : IMapper<EstablishmentResults?, List<ViewModels.Establishment>?>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input">
        /// 
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public List<ViewModels.Establishment>? MapFrom(EstablishmentResults? input)
        {
            List<ViewModels.Establishment>? searchItems = null;

            if (input != null)
            {
                searchItems = [];

                foreach (var establishment in input.Establishments)
                {
                    searchItems.Add(new ViewModels.Establishment
                    {
                        Urn = establishment.Urn,
                        Name = establishment.Name,
                        Address = new ViewModels.Address()
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
