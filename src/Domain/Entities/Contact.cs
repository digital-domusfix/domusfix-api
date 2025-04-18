using DomusFix.Api.Domain.Entities.Jobs;

namespace DomusFix.Api.Domain.Entities;
public class Contact
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;
    public string? Address { get; set; }
    public string? PostalCode { get; set; }

    public string? UserId { get; set; } // optional link to ApplicationUser (nullable for guests)

    public ICollection<Job> Jobs { get; set; } = new List<Job>();
}

