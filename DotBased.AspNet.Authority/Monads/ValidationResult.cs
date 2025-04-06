using DotBased.Monads;

namespace DotBased.AspNet.Authority.Monads;

public class ValidationResult<T> : DotBased.Monads.Result<T>
{
    private ValidationResult(T result) : base(result)
    {
    }

    private ValidationResult(Exception exception) : base(exception)
    {
    }

    private ValidationResult(ResultInformation information) : base(information)
    {
    }
    
    
}