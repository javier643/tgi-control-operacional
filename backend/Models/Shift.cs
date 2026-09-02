namespace TgiControl.Models;

public class Shift
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string ShiftType { get; set; } = string.Empty; // Morning, Afternoon, Night
    public string OperationalCenter { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public int HeadCount { get; set; }
    public string HandoverNotes { get; set; } = string.Empty;
    public string DeliveredBy { get; set; } = string.Empty;
    public string ReceivedBy { get; set; } = string.Empty;
    public DateTime? DeliveryTime { get; set; }
    public DateTime? ReceiptTime { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}