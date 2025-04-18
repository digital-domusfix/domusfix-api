namespace DomusFix.Api.Application.Jobs.Queries;
public class JobDto
{
    public string Id { get; set; } = default!;
    public string SelectedService { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
}

