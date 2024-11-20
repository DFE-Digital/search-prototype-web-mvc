using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Buttons;

public sealed class ButtonFactory : ComponentFactoryBase<Button>
{
    internal static IElementSelector Button => new ElementSelector(".govuk-button");
    public ButtonFactory(IDocumentQueryClientAccessor documentQueryClientAccessor) : base(documentQueryClientAccessor)
    {
    }

    public override List<Button> GetMany(QueryRequest? request = null)
    {
        QueryRequest queryRequest = new()
        {
            Query = request?.Query ?? Button,
            Scope = request?.Scope
        };

        return DocumentQueryClient.QueryMany(
            args: queryRequest,
            mapper: (part) =>
            {
                var classStyles = part.GetAttribute("class") ?? string.Empty;
                return new Button()
                {
                    ButtonType =
                            classStyles.Contains("govuk-button--secondary") ? ButtonStyleType.Secondary :
                            classStyles.Contains("gov-uk-button--warning") ? ButtonStyleType.Warning : ButtonStyleType.Primary,
                    Text = part.Text ?? string.Empty,
                    TagName = part.TagName,
                    Disabled = part.HasAttribute("disabled"),
                    Type = part.GetAttribute("type") ?? string.Empty
                };
            }).ToList();
    }
}
