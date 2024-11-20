using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.CheckboxInput;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.FieldSet;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.Form;

public record Form : IComponent
{
    public required string TagName { get; init; }
    public required IEnumerable<FieldSet.FieldSet> FieldSets { get; init; } = [];
    public required HttpMethod Method { get; init; }
    public required string Action { get; init; }
    public required bool IsFormValidatedWithHTML { get; init; }
}
