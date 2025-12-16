using Microsoft.Extensions.Options;

namespace NotificationService.Workers;

/// <summary>
/// Worker Service pour le traitement en arrière-plan
/// Exemple de traitement asynchrone et de jobs planifiés
/// </summary>
public class NotificationWorker : BackgroundService
{
    private readonly ILogger<NotificationWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public NotificationWorker(
        ILogger<NotificationWorker> logger,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NotificationWorker démarré à: {Time}", DateTimeOffset.Now);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                // Traitement périodique toutes les 30 secondes
                await ProcessNotificationsAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du traitement des notifications");
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken); // Attente plus longue en cas d'erreur
            }
        }

        _logger.LogInformation("NotificationWorker arrêté à: {Time}", DateTimeOffset.Now);
    }

    private async Task ProcessNotificationsAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var notificationService = scope.ServiceProvider.GetRequiredService<Services.INotificationService>();

        _logger.LogInformation("Traitement des notifications en cours...");

        // Récupération des notifications en attente
        var pendingNotifications = await notificationService.GetPendingNotificationsAsync(cancellationToken);

        foreach (var notification in pendingNotifications)
        {
            try
            {
                await notificationService.ProcessNotificationAsync(notification, cancellationToken);
                _logger.LogInformation("Notification traitée: {NotificationId}", notification.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors du traitement de la notification {NotificationId}", notification.Id);
                // Logique de retry pourrait être ajoutée ici
            }
        }

        _logger.LogInformation("Traitement terminé. {Count} notifications traitées", pendingNotifications.Count());
    }
}

