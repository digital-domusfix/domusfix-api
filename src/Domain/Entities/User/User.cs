using DomusFix.Api.Domain.Constants;

namespace DomusFix.Api.Domain.Entities.User;
public class User
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string Role { get; set; } = Roles.Customer;
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;

    public string? ContactId { get; set; } // ➕ This line (for linking to Contact)

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

