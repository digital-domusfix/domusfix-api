﻿using Microsoft.AspNetCore.Identity;

namespace DomusFix.Api.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; internal set; }
}

