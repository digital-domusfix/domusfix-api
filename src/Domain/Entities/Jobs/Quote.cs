using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomusFix.Api.Domain.Entities.Jobs;
public class Quote
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public decimal Amount { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Status { get; set; } = "pending"; 

    public string JobId { get; set; } = default!;
    public Job Job { get; set; } = default!;

    public string ContractorId { get; set; } = default!;
    public Contractor Contractor { get; set; } = default!;
}

