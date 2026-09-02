namespace TgiControl.Models;

public class Permit
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public PermitStatus Status { get; set; } = PermitStatus.Draft;
    public string RequestedBy { get; set; } = string.Empty;
    public string? ApprovedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}

public enum PermitStatus
{
    Draft,
    Filed,
    SSTReview,
    SupervisorApproval,
    AreaValidation,
    InExecution,
    Suspended,
    Revalidated,
    Transferred,
    Closed
}