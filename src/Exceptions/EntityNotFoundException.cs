using System.Runtime.Serialization;

namespace Exceptions;

[Serializable]
public class EntityNotFoundException : AppException
{
    public EntityNotFoundException()
    { }

    public EntityNotFoundException(string message)
        : base(message) { }

    public EntityNotFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }

    protected EntityNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}