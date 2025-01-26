using Microsoft.AspNetCore.Mvc;

namespace Vision360.DTOs;

public class RequestHeadersDto
{
    [FromHeader(Name = "X-Requested-With")]
    public string? RequestedWith { get; set; }
}