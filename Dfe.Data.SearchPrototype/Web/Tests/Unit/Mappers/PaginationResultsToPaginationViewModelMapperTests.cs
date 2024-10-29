using Dfe.Data.SearchPrototype.Web.Mappers;
using Dfe.Data.SearchPrototype.Web.Models.ViewModels.Shared;
using FluentAssertions;
using Xunit;

namespace Dfe.Data.SearchPrototype.Web.Tests.Unit.Mappers
{
    public sealed class PaginationResultsToPaginationViewModelMapperTests
    {
        private readonly PaginationResultsToPaginationViewModelMapper _mapper = new(new ScrollablePager());
    }
}
