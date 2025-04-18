using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomusFix.Api.Domain.Entities.Jobs;
public class Contractor
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Phone { get; set; } = default!;

    // Navigation
    public ICollection<Quote> Quotes { get; set; } = new List<Quote>();
    public ICollection<Job> AssignedJobs { get; set; } = new List<Job>();
}
