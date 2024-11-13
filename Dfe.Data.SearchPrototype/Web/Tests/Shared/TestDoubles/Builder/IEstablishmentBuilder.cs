using Dfe.Data.SearchPrototype.Infrastructure.DataTransferObjects;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles.Builder;
public interface IEstablishmentBuilder
{
    IEstablishmentBuilder SetName(string establishmentName);
    IEstablishmentBuilder SetTypeOfEstablishment(string typeOfEstablishment);
    IEstablishmentBuilder SetId(string id);
    IEstablishmentBuilder SetPhaseOfEducation(string phaseOfEducation);
    IEstablishmentBuilder SetStatus(string status);
    IEstablishmentBuilder SetAddress(string address);
    Establishment Build();
}