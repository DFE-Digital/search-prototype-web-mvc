using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.FieldSet;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.Form;

public sealed class FormComponentFactory : ComponentFactoryBase<Form>
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;
    private readonly FieldSetComponentFactory _fieldSetComponent;

    public FormComponentFactory(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        FieldSetComponentFactory fieldSetComponent) : base(documentQueryClientAccessor)
    {
        _documentQueryClientAccessor = documentQueryClientAccessor;
        _fieldSetComponent  = fieldSetComponent;
    }

    // may not be appropriate if there are multiple forms on the page
    internal IElementSelector DefaultFormQuery = new ElementSelector("form");

    public override List<Form> GetMany(QueryRequest? request = null)
    {
        IElementSelector? scope = request?.Scope;
        QueryRequest queryRequest = new(
            query: request?.Query ?? DefaultFormQuery, 
            scope);

        return DocumentQueryClient.QueryMany(
            queryRequest,
            mapper: (part) => new Form()
            {
                TagName = part.TagName,
                Method = HttpMethod.Parse(part.GetAttribute("method")
                    ?? throw new ArgumentNullException(nameof(Form.Method), "method on form is null")),
                FieldSets = _fieldSetComponent.GetMany(),
                Action = part.GetAttribute("action")
                    ?? throw new ArgumentNullException(nameof(Form.Action), "action on form is null"),
                IsFormValidatedWithHTML = part.HasAttribute("novalidate")
            })
            .ToList();
    }
}
