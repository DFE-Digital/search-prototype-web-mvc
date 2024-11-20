namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.CheckboxInput;

public sealed class CheckboxWithLabelComponent
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;
    internal static IElementSelector Checkbox => new ElementSelector(".govuk-checkboxes__item");
    internal static IElementSelector Input => new ElementSelector(".govuk-checkboxes__input");
    internal static IElementSelector Label => new ElementSelector(".govuk-checkboxes__label");

    public CheckboxWithLabelComponent(IDocumentQueryClientAccessor documentQueryClientAccessor)
    {
        _documentQueryClientAccessor = documentQueryClientAccessor;
    }

    // TODO Checkbox component
    // TODO label component
    private static Func<IDocumentPart, CheckboxWithLabel> MapCheckboxes =>
        (checkboxItemWithLabel) =>
        {
            var checkboxItem = checkboxItemWithLabel.GetChild(Input) ?? throw new ArgumentNullException(nameof(Input));
            var checkboxLabel = checkboxItemWithLabel.GetChild(Label) ?? throw new ArgumentNullException(nameof(Label));

            return new CheckboxWithLabel()
            {
                TagName = "",
                Name = checkboxItem.GetAttribute("name") ?? throw new ArgumentNullException($"no name of {nameof(checkboxItem)}")!,
                Label = checkboxLabel.Text ?? throw new ArgumentNullException($"no label on {nameof(checkboxItem)}"),
                Value = checkboxItem.GetAttribute("value") ?? throw new ArgumentNullException($"no value on {nameof(checkboxItem)}"),
                Checked = checkboxItem.HasAttribute("checked")
            };
        };

    public IEnumerable<CheckboxWithLabel> GetCheckboxes(IElementSelector? scope = null)
    {
        QueryRequest queryRequest = new(Checkbox, scope);

        return _documentQueryClientAccessor.DocumentQueryClient.QueryMany(
                args: queryRequest,
                mapper: MapCheckboxes);
    }

    public IEnumerable<CheckboxWithLabel> GetCheckboxesFromPart(IDocumentPart? part)
        => part?
            .GetChildren(Checkbox)?
            .Select(MapCheckboxes)
            .ToList() ?? throw new ArgumentNullException(nameof(part));
}
