using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.CheckboxWithLabel;

namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.FieldSet;

public sealed class FieldsetFactory : ComponentFactoryBase<Fieldset>
{
    private readonly CheckboxWithLabelFactory _checkboxWithLabelComponent;

    public FieldsetFactory(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        CheckboxWithLabelFactory checkboxWithLabelComponent) : base(documentQueryClientAccessor)
    {
        _checkboxWithLabelComponent = checkboxWithLabelComponent;
    }

    public override List<Fieldset> GetMany(QueryRequest? request = null)
    {
        QueryRequest queryRequest =
            new()
            {
                Query = request?.Query ?? new ElementSelector("fieldset"),
                Scope = request?.Scope
            };

        return DocumentQueryClient.QueryMany(
            queryRequest,
            mapper:
                (part) => new Fieldset()
                {
                    TagName = part.TagName,
                    Legend = part.GetChild(new ElementSelector("legend"))?.Text ?? throw new ArgumentNullException("legend on fieldset is null"),
                    Checkboxes = _checkboxWithLabelComponent.GetCheckboxesFromPart(part)
                }).ToList();
    }
}
