using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.CheckboxInput;

namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.FieldSet;

public sealed class FieldSetComponent : ComponentBase
{
    private readonly CheckboxWithLabelComponent _checkboxWithLabelComponent;

    public FieldSetComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        CheckboxWithLabelComponent checkboxWithLabelComponent) : base(documentQueryClientAccessor)
    {
        _checkboxWithLabelComponent = checkboxWithLabelComponent;
    }

    private IEnumerable<CheckboxWithLabel> Checkboxes => _checkboxWithLabelComponent.GetCheckboxes();

    public IEnumerable<FieldSet> GetFieldSets(IElementSelector? query, IElementSelector? scope = null)
    {
        return DocumentQueryClient.QueryMany(
            new QueryRequest(query ?? new ElementSelector("fieldset"), scope),
            mapper: 
                (part) => new FieldSet()
            {
                    Legend = part.GetChild(new ElementSelector("legend"))?.Text ?? throw new ArgumentNullException("legend on fieldset is null"),
                    Checkboxes = _checkboxWithLabelComponent.GetCheckboxesFromPart(part)
            }).ToList();
    }
}
