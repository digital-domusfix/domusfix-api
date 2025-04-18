namespace Domusfix.Domain.Entities
{
    public enum JobStatus { Pending, Accepted, InProgress, Completed, Cancelled }

    public class JobRequest
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid? ProviderId { get; set; }
        public Guid ServiceCategoryId { get; set; }
        public string Description { get; set; } =  string.Empty;
        public string Location { get; set; } =  string.Empty;
        public JobStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}
