namespace DfE.Data.SearchPrototype.Web.Tests.Shared.Pages.Components.Input;

public record Input
{
    public required string Name { get; init; }
    public required string Value { get; init; }
    public required string? PlaceHolder { get; init; } = null;
    public required string? Type { get; init; } = null;
}
