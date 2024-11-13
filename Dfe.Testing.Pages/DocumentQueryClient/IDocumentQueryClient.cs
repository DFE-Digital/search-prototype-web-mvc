namespace Dfe.Testing.Pages.DocumentQueryClient;
public interface IDocumentQueryClient
{
    TResult Query<TResult>(QueryCommand<TResult> queryCommand);
    IEnumerable<TResult> QueryMany<TResult>(QueryCommand<TResult> queryCommand);
}
