using System;

namespace Domain.Exceptions;

public class ConflictException : BusinessException
{
    public ConflictException(string message) : base(message) { }
}
