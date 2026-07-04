using Estapar.Domain.Dtos.Results;

namespace Estapar.Domain.Exceptions.Base;

/// <summary>
/// Represents the base class for exceptions that occur during application execution.
/// </summary>
/// <remarks>This class serves as a foundation for creating custom exceptions in the application. It extends the
/// <see cref="Exception"/> class and provides a property to hold additional error information.</remarks>
public abstract class BaseException : Exception
{
    /// <summary>
    /// ctor
    /// </summary>
    public BaseException() { }

    /// <summary>
    /// Gets or sets the error result associated with the current operation.
    /// </summary>
    public ErrorResult Response { get; set; }
}
