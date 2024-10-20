namespace DfE.Data.SearchPrototype.Pages;

public sealed class HomePage : BasePage
{
    public HomePage(IDomQueryClient domQueryClient) : base(domQueryClient)
    {
        
    }

    // TODO improve locator?
    public string GetHeading() => DomQueryClient.GetText("header div div:nth-of-type(2) a");
}

public abstract class BasePage
{

    protected BasePage(IDomQueryClient domQueryClient)
    {
        ArgumentNullException.ThrowIfNull(domQueryClient);
        DomQueryClient = domQueryClient;
    }

    protected IDomQueryClient DomQueryClient { get; }
}
