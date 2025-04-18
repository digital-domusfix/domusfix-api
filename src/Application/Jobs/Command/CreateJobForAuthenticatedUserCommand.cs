using DomusFix.Api.Application.Common.Models;

public class CreateJobForAuthenticatedUserCommand : IRequest<Result<string>>
{
    public string SelectedService { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public DateTime PreferredDate { get; set; }

    // Optional user-provided contact override
    public string? Name { get; set; }
    public string? Phone { get; set; }
}
