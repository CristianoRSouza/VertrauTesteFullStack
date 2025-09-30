using System;

namespace Domain.Exceptions;

public class ForbiddenException : BusinessException
{
    public ForbiddenException(string message) : base(message) { }
}
