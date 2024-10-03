using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.SearchForEstablishments.ByKeyword.Usecase;
using Moq;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles
{
    public static class ViewModelSelectedFacetsToFilterRequestMapperTestDouble
    {
        public static Mock<IMapper<Dictionary<string, List<string>>?, IList<FilterRequest>>> MockFor(IList<FilterRequest> filterRequests)
        {
            Mock<IMapper<Dictionary<string, List<string>>?, IList<FilterRequest>>> mockMapper = DefaultMock();

            mockMapper.Setup(mapper =>
                mapper.MapFrom(It.IsAny<Dictionary<string, List<string>>>())).Returns(filterRequests);

            return mockMapper;
        }

        public static Mock<IMapper<Dictionary<string, List<string>>?, IList<FilterRequest>>> DefaultMock() => new();
    }
}
