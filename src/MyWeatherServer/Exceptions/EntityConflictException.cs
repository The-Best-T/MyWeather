using System.Runtime.Serialization;

namespace Exceptions;

[Serializable]
public class EntityConflictException : AppException
{
    public EntityConflictException() { }

    public EntityConflictException(string message) : base(message) { }

    protected EntityConflictException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
}