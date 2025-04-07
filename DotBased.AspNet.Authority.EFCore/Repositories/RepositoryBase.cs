namespace DotBased.AspNet.Authority.EFCore.Repositories;

public abstract class RepositoryBase
{
    protected ResultOld<T> HandleExceptionResult<T>(string message, Exception ex) => new(HandleException(message, ex));

    protected ListResultOld<T> HandleExceptionListResult<T>(string message, Exception ex) =>
        new(HandleException(message, ex));

    protected ResultOld HandleException(string message, Exception ex)
    {
        if (ex is OperationCanceledException oce)
        {
            return ResultOld.Failed("Operation cancelled.", oce);
        }

        return ResultOld.Failed(message, ex);
    }
}