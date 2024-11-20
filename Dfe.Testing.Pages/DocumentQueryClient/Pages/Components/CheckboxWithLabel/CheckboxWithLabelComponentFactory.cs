namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.CheckboxInput;

public sealed class CheckboxWithLabelComponentFactory : ComponentFactoryBase<CheckboxWithLabel>
{
    internal static IElementSelector Checkbox => new ElementSelector(".govuk-checkboxes__item");
    internal static IElementSelector Input => new ElementSelector(".govuk-checkboxes__input");
    internal static IElementSelector Label => new ElementSelector(".govuk-checkboxes__label");

    public CheckboxWithLabelComponentFactory(IDocumentQueryClientAccessor documentQueryClientAccessor) : base(documentQueryClientAccessor)
    {
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

    public override List<CheckboxWithLabel> GetMany(QueryRequest? request = null)
    {
        IElementSelector? scope = request?.Scope;
        QueryRequest queryRequest = new(
            query: request?.Query ?? Checkbox,
            scope);

        return DocumentQueryClient.QueryMany(
                args: queryRequest,
                mapper: MapCheckboxes)
            .ToList();
    }

    internal List<CheckboxWithLabel> GetCheckboxesFromPart(IDocumentPart? part)
        => part?
            .GetChildren(Checkbox)?
            .Select(MapCheckboxes)
            .ToList() ?? throw new ArgumentNullException(nameof(part));
}
