using System;

namespace Domain.Exceptions;

public class UnauthorizedException : BusinessException
{
    public UnauthorizedException(string message) : base(message) { }
}
