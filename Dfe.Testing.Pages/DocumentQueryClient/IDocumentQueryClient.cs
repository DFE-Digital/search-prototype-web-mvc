namespace Dfe.Testing.Pages.DocumentQueryClient;
public interface IDocumentQueryClient
{
    void Run(QueryArgs args, Action<IDocumentPart> handler);
    TResult Query<TResult>(QueryArgs args, Func<IDocumentPart, TResult> mapper);
    IEnumerable<TResult> QueryMany<TResult>(QueryArgs args, Func<IDocumentPart, TResult> mapper);
}
