namespace Dfe.Testing.Pages.DocumentQueryClient;
public interface IDocumentQueryClient
{
    void Run(ElementQueryArguments args, Action<IDocumentPart> handler);
    TResult Query<TResult>(ElementQueryArguments args, Func<IDocumentPart, TResult> mapper);
    IEnumerable<TResult> QueryMany<TResult>(ElementQueryArguments args, Func<IDocumentPart, TResult> mapper);
}
