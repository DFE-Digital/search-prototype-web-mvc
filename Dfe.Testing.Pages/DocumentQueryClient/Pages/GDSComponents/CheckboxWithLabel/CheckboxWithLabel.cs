using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents;

namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.CheckboxWithLabel;

public sealed record CheckboxWithLabel : IComponent
{
    public required string TagName { get; init; }
    public required string Name { get; init; }
    public required string Value { get; init; }
    public required string Label { get; init; }
    public bool Checked { get; init; } = false;
}
