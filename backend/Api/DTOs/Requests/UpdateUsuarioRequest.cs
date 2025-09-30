using System;

namespace Api.DTOs.Requests;

public class UpdateUsuarioRequest : CreateUsuarioRequest
{
    public long Id { get; set; }
}
