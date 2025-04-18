using System.Reflection;
using DomusFix.Api.Application.Common.Interfaces;
using DomusFix.Api.Domain.Entities;
using DomusFix.Api.Domain.Entities.Jobs;
using DomusFix.Api.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DomusFix.Api.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Quote> Quotes => Set<Quote>();
    public DbSet<Contractor> Contractors => Set<Contractor>();
    public DbSet<Contact> Contacts => Set<Contact>(); // 👈 NEW

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // One-to-many: Job has many Quotes
        builder.Entity<Job>()
            .HasMany(j => j.Quotes)
            .WithOne(q => q.Job)
            .HasForeignKey(q => q.JobId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-many: Contractor has many Quotes
        builder.Entity<Contractor>()
            .HasMany(c => c.Quotes)
            .WithOne(q => q.Contractor)
            .HasForeignKey(q => q.ContractorId)
            .OnDelete(DeleteBehavior.Cascade);

        // One-to-many: Contractor has many AssignedJobs
        builder.Entity<Contractor>()
            .HasMany(c => c.AssignedJobs)
            .WithOne(j => j.Contractor)
            .HasForeignKey(j => j.ContractorId)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-one: Job has one AcceptedQuote
        builder.Entity<Job>()
            .HasOne(j => j.AcceptedQuote)
            .WithMany()
            .HasForeignKey(j => j.AcceptedQuoteId)
            .OnDelete(DeleteBehavior.Restrict);

        // One-to-many: User has many Contacts
        builder.Entity<Contact>()
            .HasMany(c => c.Jobs)
            .WithOne(j => j.Contact)
            .HasForeignKey(j => j.ContactId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.ConfigureWarnings(builder => builder.Ignore(RelationalEventId.PendingModelChangesWarning));
        base.OnConfiguring(optionsBuilder);
    }
}
