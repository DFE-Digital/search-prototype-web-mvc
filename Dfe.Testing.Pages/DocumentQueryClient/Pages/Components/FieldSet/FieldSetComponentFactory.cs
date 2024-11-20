using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.CheckboxInput;

namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.FieldSet;

public sealed class FieldSetComponentFactory : ComponentFactoryBase<FieldSet>
{
    private readonly CheckboxWithLabelComponentFactory _checkboxWithLabelComponent;

    public FieldSetComponentFactory(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        CheckboxWithLabelComponentFactory checkboxWithLabelComponent) : base(documentQueryClientAccessor)
    {
        _checkboxWithLabelComponent = checkboxWithLabelComponent;
    }

    private IEnumerable<CheckboxWithLabel> Checkboxes => _checkboxWithLabelComponent.GetMany();

    public override List<FieldSet> GetMany(QueryRequest? request = null)
    {
        IElementSelector? scope = request?.Scope;
        QueryRequest queryRequest = 
            new(
                query: request?.Query ?? new ElementSelector("fieldset"),
                scope);

        return DocumentQueryClient.QueryMany(
            queryRequest,
            mapper:
                (part) => new FieldSet()
                {
                    TagName = part.TagName,
                    Legend = part.GetChild(new ElementSelector("legend"))?.Text ?? throw new ArgumentNullException("legend on fieldset is null"),
                    Checkboxes = _checkboxWithLabelComponent.GetCheckboxesFromPart(part)
                }).ToList();
    }
}
