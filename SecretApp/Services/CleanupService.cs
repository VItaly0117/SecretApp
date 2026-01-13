using SecretApp.Data;

public class CleanupService : BackgroundService
{
    private readonly IServiceProvider _services;
    public CleanupService(IServiceProvider services) => _services = services;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var oldMessages = db.Messages.Where(m => m.CreatedAt < DateTime.UtcNow.AddHours(-24));
                db.Messages.RemoveRange(oldMessages);
                await db.SaveChangesAsync();
            }
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken); // Чистим раз в час
        }
    }
}