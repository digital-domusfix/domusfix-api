namespace DomusFix.Api.Domain.Entities.Jobs;
public class Job
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public string SelectedService { get; set; } = default!;
    public string Status { get; set; } = "pending_quote";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 
        public DateTime PreferredDate { get; set; }
    public string? UserId { get; set; } // if created by logged-in user

    public string? ContactId { get; set; } // can be guest or user’s contact
    public Contact? Contact { get; set; }

    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
    public string? AcceptedQuoteId { get; set; }
    public Quote? AcceptedQuote { get; set; }

    public string? ContractorId { get; set; }
    public Contractor? Contractor { get; set; }
}



