namespace NotificationService.Models;

/// <summary>
/// Modèle de notification
/// Représente une notification à traiter
/// </summary>
public class Notification
{
    public Guid Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Recipient { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public NotificationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ProcessedAt { get; set; }
    public int RetryCount { get; set; }
}

public enum NotificationType
{
    Email,
    Sms,
    Push
}

public enum NotificationStatus
{
    Pending,
    Processing,
    Sent,
    Failed
}

