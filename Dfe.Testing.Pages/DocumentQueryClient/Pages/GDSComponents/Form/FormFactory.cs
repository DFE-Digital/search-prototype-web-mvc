using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Buttons;
using Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.FieldSet;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Form;

public sealed class FormFactory : ComponentFactoryBase<Form>
{
    // may not be appropriate if there are multiple forms on the page
    internal static IElementSelector DefaultFormQuery = new ElementSelector("form");
    private readonly FieldsetFactory _fieldSetFactory;
    private readonly ButtonFactory _buttonFactory;

    public FormFactory(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        FieldsetFactory fieldSetComponent,
        ButtonFactory buttonFactory) : base(documentQueryClientAccessor)
    {
        ArgumentNullException.ThrowIfNull(fieldSetComponent);
        ArgumentNullException.ThrowIfNull(buttonFactory);
        _fieldSetFactory = fieldSetComponent;
        _buttonFactory = buttonFactory;
    }

    public override List<Form> GetMany(QueryRequest? request = null)
    {
        QueryRequest queryRequest = new()
        {
            Query = request?.Query ?? DefaultFormQuery,
            Scope = request?.Scope
        };

        return DocumentQueryClient.QueryMany(
            queryRequest,
            mapper: (part) => new Form()
            {
                TagName = part.TagName,
                Method = HttpMethod.Parse(part.GetAttribute("method") ?? throw new ArgumentNullException(nameof(Form.Method), "method on form is null")),
                FieldSets = _fieldSetFactory.GetMany(new QueryRequest() { Scope = queryRequest.Scope }),
                Buttons = _buttonFactory.GetMany(new QueryRequest() { Scope = queryRequest.Scope }),
                Action = part.GetAttribute("action") ?? throw new ArgumentNullException(nameof(Form.Action), "action on form is null"),
                IsFormValidatedWithHTML = part.HasAttribute("novalidate")
            })
            .ToList();
    }
}
