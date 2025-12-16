using NotificationService.Services;
using NotificationService.Workers;
using Serilog;

// Configuration de Serilog pour le logging structuré
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Démarrage du NotificationService");

    var builder = Host.CreateApplicationBuilder(args);

    // Configuration de Serilog
    builder.Host.UseSerilog();

    // Configuration des services
    builder.Services.AddScoped<INotificationService, NotificationService.Services.NotificationService>();

    // Configuration du Worker Service
    builder.Services.AddHostedService<NotificationWorker>();

    var host = builder.Build();

    // Création de la table Notifications (pour le développement)
    await EnsureDatabaseCreatedAsync(host.Services);

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Le NotificationService s'est arrêté de manière inattendue");
}
finally
{
    Log.CloseAndFlush();
}

static async Task EnsureDatabaseCreatedAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("DefaultConnection");

    if (string.IsNullOrEmpty(connectionString))
    {
        Log.Warning("ConnectionString non configurée, la base de données ne sera pas créée");
        return;
    }

    try
    {
        using var connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
        await connection.OpenAsync();

        // Création de la table Notifications si elle n'existe pas
        var createTableSql = @"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Notifications]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[Notifications] (
                    [Id] UNIQUEIDENTIFIER PRIMARY KEY,
                    [Message] NVARCHAR(MAX) NOT NULL,
                    [Recipient] NVARCHAR(255) NOT NULL,
                    [Type] NVARCHAR(50) NOT NULL,
                    [Status] NVARCHAR(50) NOT NULL,
                    [CreatedAt] DATETIME2 NOT NULL,
                    [ProcessedAt] DATETIME2 NULL,
                    [RetryCount] INT NOT NULL DEFAULT 0
                );
            END";

        using var command = new Microsoft.Data.SqlClient.SqlCommand(createTableSql, connection);
        await command.ExecuteNonQueryAsync();

        Log.Information("Base de données initialisée avec succès");
    }
    catch (Exception ex)
    {
        Log.Warning(ex, "Impossible de créer la table Notifications. Elle sera créée lors de la première utilisation.");
    }
}
