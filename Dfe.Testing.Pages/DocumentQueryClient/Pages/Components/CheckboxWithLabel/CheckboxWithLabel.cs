namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.CheckboxInput;

public sealed record CheckboxWithLabel : IComponent
{
    public required string Name { get; init; }
    public required string Value { get; init; }
    public required string Label { get; init; }
    public required string TagName { get; init; }
    public bool Checked { get; init; } = false;
}
