using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.Models;

namespace Dfe.Data.SearchPrototype.Web.Mappers
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EstablishmentResultsToEstablishmentsViewModelMapper : IMapper<EstablishmentResults?, List<Models.Establishment>?>
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
        public List<Models.Establishment>? MapFrom(EstablishmentResults? input)
        {
            List<Models.Establishment>? searchItems = null;

            if (input != null)
            {
                searchItems = [];

                foreach (var establishment in input.Establishments)
                {
                    searchItems.Add(new Models.Establishment
                    {
                        Urn = establishment.Urn,
                        Name = establishment.Name,
                        Address = new Models.Address()
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
