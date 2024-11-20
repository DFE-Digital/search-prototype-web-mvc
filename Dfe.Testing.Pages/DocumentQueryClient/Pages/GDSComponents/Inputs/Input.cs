namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Inputs;
public record Input
{
    public required string Name { get; init; }
    public required string Value { get; init; }
    public required string? PlaceHolder { get; init; } = null;
    public required string? Type { get; init; } = null;
}