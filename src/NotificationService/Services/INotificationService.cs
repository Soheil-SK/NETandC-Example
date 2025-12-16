using NotificationService.Models;

namespace NotificationService.Services;

/// <summary>
/// Interface pour le service de notifications
/// Gestion des notifications et traitement en arrière-plan
/// </summary>
public interface INotificationService
{
    Task<IEnumerable<Notification>> GetPendingNotificationsAsync(CancellationToken cancellationToken = default);
    Task ProcessNotificationAsync(Notification notification, CancellationToken cancellationToken = default);
    Task CreateNotificationAsync(string message, string recipient, NotificationType type, CancellationToken cancellationToken = default);
}

