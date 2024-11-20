namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.FieldSet;

public record Fieldset : IComponent
{
    public required string TagName { get; init; }
    public required string Legend { get; init; }
    public required IEnumerable<CheckboxWithLabel.CheckboxWithLabel> Checkboxes { get; init; }
}
