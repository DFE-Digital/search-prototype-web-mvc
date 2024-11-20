using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.CheckboxInput;

namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.FieldSet;

public record FieldSet
{
    public required string Legend { get; init; }
    public required IEnumerable<CheckboxWithLabel> Checkboxes { get; init; }
}
