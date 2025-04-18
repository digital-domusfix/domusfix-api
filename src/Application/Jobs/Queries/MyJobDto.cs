namespace DomusFix.Api.Application.Jobs.Queries;
public class MyJobDto
{
    public string Id { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Type { get; set; } = default!;
    public string PreferredDate { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }

    public QuoteDto? Quote { get; set; }
    public ContractorDto? Contractor { get; set; }
}
