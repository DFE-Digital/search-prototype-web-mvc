using Dfe.Data.SearchPrototype.Common.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using Moq;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.TestDoubles
{
    public sealed class PaginationMapperTestDouble
    {
        public static Mock<IMapper<(int, int), Pagination>> MockFor(Pagination response)
        {
            Mock<IMapper<(int, int), Pagination>> mockMapper = DefaultMock();

            mockMapper.Setup(mapper =>
                mapper.MapFrom(It.IsAny<(int, int)>())).Returns(response);

            return mockMapper;
        }

        public static Mock<IMapper<(int, int), Pagination>> DefaultMock() => new();
    }
}
