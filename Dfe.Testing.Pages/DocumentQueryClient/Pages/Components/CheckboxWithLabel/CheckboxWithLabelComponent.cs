namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.CheckboxInput;

public sealed class CheckboxWithLabelComponent
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;

    public CheckboxWithLabelComponent(IDocumentQueryClientAccessor documentQueryClientAccessor)
    {
        _documentQueryClientAccessor = documentQueryClientAccessor;
    }

    // TODO Checkbox component
    // TODO label component
    private static Func<IDocumentPart, CheckboxWithLabel> MapCheckboxes =>
        (checkboxItemWithLabel) =>
        {
            return new CheckboxWithLabel()
            {
                TagName = "",
                Name = checkboxItemWithLabel.GetChild(new ElementSelector(".govuk-checkboxes__input"))?.GetAttribute("name") ?? throw new ArgumentNullException("name")!,
                Label = checkboxItemWithLabel.GetChild(new ElementSelector(".govuk-checkboxes__label"))?.Text ?? throw new ArgumentNullException("label"),
                Value = checkboxItemWithLabel.GetChild(new ElementSelector(".govuk-checkboxes__input"))?.GetAttribute("value") ?? throw new ArgumentNullException("value"),
                Checked = checkboxItemWithLabel.GetChild(new ElementSelector(".govuk-checkboxes__input"))?.HasAttribute("checked") ?? throw new ArgumentNullException("checked")
            };
        };

    public IEnumerable<CheckboxWithLabel> GetCheckboxes(IElementSelector? scope = null)
    {
        IElementSelector checkboxWithLabelSelector = new ElementSelector(".govuk-checkboxes__item");

        QueryRequest queryRequest = new(checkboxWithLabelSelector, scope);

        return _documentQueryClientAccessor.DocumentQueryClient.QueryMany(
                args: queryRequest,
                mapper: MapCheckboxes);
    }

    public IEnumerable<CheckboxWithLabel> GetCheckboxesFromPart(IDocumentPart? part)
        => part?
            .GetChildren(new ElementSelector(".govuk-checkboxes__item"))
            ?.Select(MapCheckboxes)
            .ToList() ?? throw new ArgumentNullException(nameof(part));
}
