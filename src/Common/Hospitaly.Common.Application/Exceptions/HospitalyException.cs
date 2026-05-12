using ErrorOr;

namespace Hospitaly.Common.Application.Exceptions;

public class HospitalyException : Exception
{
    public HospitalyException(string requestName, Error? error = default, Exception? innerException = default)
        : base("Application exception", innerException)
    {
        RequestName = requestName;
        Error = error;
    }

    public string RequestName { get; }

    public Error? Error { get; }
}