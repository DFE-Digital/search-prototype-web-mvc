using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Buttons;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Form;

public record Form : IComponent
{
    public required string TagName { get; init; }
    public required IEnumerable<FieldSet.Fieldset> FieldSets { get; init; } = [];
    public required IEnumerable<Button> Buttons { get; init; } = [];
    public required HttpMethod Method { get; init; }
    public required string Action { get; init; }
    public required bool IsFormValidatedWithHTML { get; init; }
}
