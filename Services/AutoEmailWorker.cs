using ClinicResearchWebApp.Data;
using ClinicResearchWebApp.Models;

namespace ClinicResearchWebApp.Services
{
    public class AutoEmailWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public AutoEmailWorker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    // Simulate sending a monthly health check email
                    db.HealthCheckAlerts.Add(new HealthCheckAlert
                    {
                        Recipient = "participant.trial01@example.com",
                        Status = "Sent (Automated Monthly Check)",
                        SentAt = DateTime.UtcNow
                    });

                    db.AuditLogs.Add(new AuditLog
                    {
                        UserEmail = "AutoEmailWorker [System]",
                        Action = "Dispatched monthly health check notification",
                        Timestamp = DateTime.UtcNow
                    });

                    await db.SaveChangesAsync(stoppingToken);
                }

                // Runs every 60 seconds for demonstration purposes
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}

