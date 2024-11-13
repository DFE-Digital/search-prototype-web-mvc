using Dfe.Data.SearchPrototype.Infrastructure.DataTransferObjects;

namespace Dfe.Data.SearchPrototype.Web.Tests.Shared.TestDoubles.Builder;
public sealed class EstablishmentBuilder : IEstablishmentBuilder
{
    private string? _establishmentName = null;
    private string? _id = null;
    private string? _typeOfEstablishmentName = null;
    private string? _phaseOfEducation = null;
    private string? _establishmentStatus = null;
    private string? _address = null;
    public EstablishmentBuilder()
    {

    }
    public Establishment Build()
        => new()
        {
            ESTABLISHMENTNAME = _establishmentName,
            id = _id,
            TYPEOFESTABLISHMENTNAME = _typeOfEstablishmentName,
            PHASEOFEDUCATION = _phaseOfEducation,
            ESTABLISHMENTSTATUSNAME = _establishmentStatus,

        };

    public IEstablishmentBuilder SetName(string establishmentName)
    {
        _establishmentName = establishmentName;
        return this;
    }
    public IEstablishmentBuilder SetStatus(string establishmentStatus)
    {
        _establishmentStatus = establishmentStatus;
        return this;
    }

    public IEstablishmentBuilder SetId(string id)
    {
        _id = id;
        return this;
    }

    public IEstablishmentBuilder SetPhaseOfEducation(string phaseOfEducation)
    {
        _phaseOfEducation = phaseOfEducation;
        return this;
    }

    public IEstablishmentBuilder SetTypeOfEstablishment(string typeOfEstablishmentName)
    {
        _typeOfEstablishmentName = typeOfEstablishmentName;
        return this;
    }

    public IEstablishmentBuilder SetAddress(string address)
    {
        _address = address;
        return this;
    }
}