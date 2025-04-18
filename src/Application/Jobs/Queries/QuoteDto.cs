namespace DomusFix.Api.Application.Jobs.Queries;
public class QuoteDto
{
    public decimal Amount { get; set; }
    public string Message { get; set; } = string.Empty;
    public ContractorDto Contractor { get; set; } = default!;
}
