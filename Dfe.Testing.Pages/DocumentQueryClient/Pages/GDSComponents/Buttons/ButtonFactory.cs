using System.Linq;

namespace Dfe.Testing.Pages.DocumentQueryClient.Pages.GDSComponents.Buttons;

public sealed class ButtonFactory : ComponentFactoryBase<Button>
{
    internal static IElementSelector Button => new ElementSelector(".govuk-button");
    internal static IElementSelector SecondaryButtonStyle => new ElementSelector("govuk-button--secondary");
    internal static IElementSelector WarningButtonStyle => new ElementSelector(".govuk-button--warning");
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
                        classStyles.Contains(SecondaryButtonStyle.ToSelector()) ? ButtonStyleType.Secondary :
                        classStyles.Contains(WarningButtonStyle.ToSelector()) ? ButtonStyleType.Warning : ButtonStyleType.Primary,
                    Text = part.Text?.Trim() ?? string.Empty,
                    TagName = part.TagName,
                    Disabled = part.HasAttribute("disabled"),
                    Type = part.GetAttribute("type") ?? string.Empty
                };
            }).ToList();
    }
}
