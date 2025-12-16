using Dapper;
using Microsoft.Data.SqlClient;
using NotificationService.Models;

namespace NotificationService.Services;

/// <summary>
/// Service de notifications utilisant Dapper pour l'accès aux données
/// Exemple d'utilisation de Dapper pour des requêtes optimisées
/// </summary>
public class NotificationService : INotificationService
{
    private readonly string _connectionString;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(
        IConfiguration configuration,
        ILogger<NotificationService> logger)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionString non configurée");
        _logger = logger;
    }

    public async Task<IEnumerable<Notification>> GetPendingNotificationsAsync(CancellationToken cancellationToken = default)
    {
        const string sql = @"
            SELECT Id, Message, Recipient, Type, Status, CreatedAt, ProcessedAt, RetryCount
            FROM Notifications
            WHERE Status = @Status
            ORDER BY CreatedAt ASC";

        using var connection = new SqlConnection(_connectionString);
        var notifications = await connection.QueryAsync<Notification>(
            sql,
            new { Status = NotificationStatus.Pending });

        return notifications;
    }

    public async Task ProcessNotificationAsync(Notification notification, CancellationToken cancellationToken = default)
    {
        // Mise à jour du statut à "Processing"
        await UpdateNotificationStatusAsync(notification.Id, NotificationStatus.Processing, cancellationToken);

        try
        {
            // Simulation du traitement de la notification
            _logger.LogInformation(
                "Envoi de {Type} à {Recipient}: {Message}",
                notification.Type,
                notification.Recipient,
                notification.Message);

            // Simuler un délai de traitement
            await Task.Delay(1000, cancellationToken);

            // Dans un vrai scénario, on appellerait un service d'envoi d'email/SMS/Push
            // await _emailService.SendAsync(notification.Recipient, notification.Message);

            // Mise à jour du statut à "Sent"
            await UpdateNotificationStatusAsync(notification.Id, NotificationStatus.Sent, cancellationToken);
            await UpdateProcessedAtAsync(notification.Id, cancellationToken);

            _logger.LogInformation("Notification {NotificationId} envoyée avec succès", notification.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du traitement de la notification {NotificationId}", notification.Id);

            // Incrémenter le compteur de retry
            await IncrementRetryCountAsync(notification.Id, cancellationToken);

            // Si trop de tentatives, marquer comme Failed
            if (notification.RetryCount >= 3)
            {
                await UpdateNotificationStatusAsync(notification.Id, NotificationStatus.Failed, cancellationToken);
            }
            else
            {
                // Remettre en Pending pour retry
                await UpdateNotificationStatusAsync(notification.Id, NotificationStatus.Pending, cancellationToken);
            }

            throw;
        }
    }

    public async Task CreateNotificationAsync(
        string message,
        string recipient,
        NotificationType type,
        CancellationToken cancellationToken = default)
    {
        const string sql = @"
            INSERT INTO Notifications (Id, Message, Recipient, Type, Status, CreatedAt, RetryCount)
            VALUES (@Id, @Message, @Recipient, @Type, @Status, @CreatedAt, @RetryCount)";

        var notification = new Notification
        {
            Id = Guid.NewGuid(),
            Message = message,
            Recipient = recipient,
            Type = type,
            Status = NotificationStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            RetryCount = 0
        };

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            sql,
            new
            {
                notification.Id,
                notification.Message,
                notification.Recipient,
                Type = notification.Type.ToString(),
                Status = notification.Status.ToString(),
                notification.CreatedAt,
                notification.RetryCount
            });

        _logger.LogInformation("Notification créée: {NotificationId}", notification.Id);
    }

    private async Task UpdateNotificationStatusAsync(
        Guid id,
        NotificationStatus status,
        CancellationToken cancellationToken)
    {
        const string sql = @"
            UPDATE Notifications
            SET Status = @Status
            WHERE Id = @Id";

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            sql,
            new { Id = id, Status = status.ToString() });
    }

    private async Task UpdateProcessedAtAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = @"
            UPDATE Notifications
            SET ProcessedAt = @ProcessedAt
            WHERE Id = @Id";

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(
            sql,
            new { Id = id, ProcessedAt = DateTime.UtcNow });
    }

    private async Task IncrementRetryCountAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = @"
            UPDATE Notifications
            SET RetryCount = RetryCount + 1
            WHERE Id = @Id";

        using var connection = new SqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, new { Id = id });
    }
}

