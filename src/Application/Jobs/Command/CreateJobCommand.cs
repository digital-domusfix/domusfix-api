using DomusFix.Api.Application.Common.Models;

namespace DomusFix.Api.Application.Jobs.Command;

public class CreateJobCommand : IRequest<Result<string>>
{
    public string SelectedService { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = default!;
    public string PostalCode { get; set; } = default!;
    public DateTime PreferredDate { get; set; }

    // Contact info (optional for logged-in users)
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
}
