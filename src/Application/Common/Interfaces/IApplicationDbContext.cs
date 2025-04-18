using DomusFix.Api.Domain.Entities;
using DomusFix.Api.Domain.Entities.Jobs;

namespace DomusFix.Api.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Job> Jobs { get;  }
    public DbSet<Quote> Quotes { get; }
    public DbSet<Contractor> Contractors { get; }
    public DbSet<Contact> Contacts { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
