using DomusFix.Api.Application.Common.Models;

namespace DomusFix.Api.Application.Authentication.Commands;
public class UpdateProfileCommand : IRequest<Result>
{
    public string Name { get; set; } = default!;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
}
