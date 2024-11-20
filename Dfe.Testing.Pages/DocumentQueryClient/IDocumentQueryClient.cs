namespace Dfe.Testing.Pages.DocumentQueryClient;
public interface IDocumentQueryClient
{
    void Run(QueryRequest args, Action<IDocumentPart> handler);
    TResult Query<TResult>(QueryRequest args, Func<IDocumentPart, TResult> mapper);
    IEnumerable<TResult> QueryMany<TResult>(QueryRequest args, Func<IDocumentPart, TResult> mapper);
}
