namespace DotBased.AspNet.Authority.EFCore.Repositories;

public abstract class RepositoryBase
{
    protected Result<T> HandleExceptionResult<T>(string message, Exception ex) => new(HandleException(message, ex));

    protected ListResult<T> HandleExceptionListResult<T>(string message, Exception ex) =>
        new(HandleException(message, ex));

    protected Result HandleException(string message, Exception ex)
    {
        if (ex is OperationCanceledException oce)
        {
            return Result.Failed("Operation cancelled.", oce);
        }

        return Result.Failed(message, ex);
    }
}