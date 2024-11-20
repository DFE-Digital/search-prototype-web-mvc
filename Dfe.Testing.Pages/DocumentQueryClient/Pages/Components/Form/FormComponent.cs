using Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.FieldSet;
using HttpMethod = System.Net.Http.HttpMethod;

namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.Components.Form;

public sealed class FormComponent : ComponentBase
{
    private readonly IDocumentQueryClientAccessor _documentQueryClientAccessor;
    private readonly FieldSetComponent _fieldSetComponent;

    public FormComponent(
        IDocumentQueryClientAccessor documentQueryClientAccessor,
        FieldSetComponent fieldSetComponent) : base(documentQueryClientAccessor)
    {
        _documentQueryClientAccessor = documentQueryClientAccessor;
        _fieldSetComponent  = fieldSetComponent;
    }

    // may not be appropriate if there are multiple forms on the page
    internal IElementSelector DefaultFormQuery = new ElementSelector("form");



    public Form Get(IElementSelector? formQuery = null, IElementSelector? scope = null)
    {
        QueryRequest queryRequest = new(formQuery ?? DefaultFormQuery, scope);
        return DocumentQueryClient.Query(
            queryRequest, 
            mapper: (part) => new Form()
            {
                Method = HttpMethod.Parse(part.GetAttribute("method") 
                    ?? throw new ArgumentNullException(nameof(Form.Method), "method on form is null")),
                FieldSets = _fieldSetComponent.GetFieldSets(scope),
                Action = part.GetAttribute("action") 
                    ?? throw new ArgumentNullException(nameof(Form.Action), "action on form is null"),
                IsFormValidatedWithHTML = part.HasAttribute("novalidate")
            });
    }
}
